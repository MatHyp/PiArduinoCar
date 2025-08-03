import subprocess
import threading
import time
import sys
import os
import socket 
from collections import deque

class ScrcpyScreenCapture:
    def __init__(self,
                tcp_host: str = "127.0.0.1",  # Host for TCP connection
                tcp_port: int = 1234,          # Port for TCP connection
                chunk_size: int = 1400,         # Size of each UDP packet
                buffer_max_chunks: int = 100,  # Max chunks in buffer
                udp_ip: str = "127.0.0.1",     # UDP target IP
                udp_port: int = 27000
            ):        # UDP target port


        # Constructor arguments into instance attributes
        self.chunk_size = chunk_size
        self.buffer_max_chunks = buffer_max_chunks  
        

        self.tcp_host = tcp_host
        self.tcp_port = tcp_port

        self.udp_ip = udp_ip
        self.udp_port = udp_port

        # H264 PIPO Buffer and Mutex (lock) to safely synchronize access 
        self.h264_buffer = deque(maxlen=buffer_max_chunks)
        self.buffer_lock = threading.Lock()
        
        # Thread-safe flag to control the lifecycle of worker threads 
        self.running = threading.Event()
        self.running.set() 

        # Placeholders for subprocess handles (scrcpy and ffmpeg)

        # Thread handles for background tasks (stderr reading, ffmpeg monitoring, and UDP sending)        
        self.tcp_reader_thread = None

        # UDP socket used to send encoded video chunks
        self.tcp_socket = None
        self.udp_socket = None



    def _read_tcp_to_buffer(self):
        """
        Continuously reads binary data from a subprocess pipe and appends it to a shared buffer in a thread-safe way.

        Parameters:
        - pipe: file-like object (e.g., subprocess stdout or stderr)
        - buffer: deque acting as a thread-safe buffer to store chunks
        - lock: threading.Lock instance for synchronizing access to the buffer
        - running_event: threading.Event used to control when reading should stop
        - name: str identifier used for logging purposes
        """
        
        while self.running.is_set():
            try:
                chunk = self.tcp_socket.recv(self.chunk_size)

                if chunk:
                    with self.buffer_lock:
                        self.h264_buffer.append(chunk)
                    print(f"[TCP_READER] Read {len(chunk)} bytes. Buffer: {len(self.h264_buffer)}/{self.h264_buffer.maxlen}")
                else:
                    # If chunk is empty, server might have closed the connection
                    print("[TCP_READER] No data received. Server may have closed the connection.")
                    time.sleep(0.5)  # prevent CPU spin
            except socket.timeout:
                # No data yet — not an error, keep going
                print("[TCP_READER] Socket timeout, no data yet...")
                continue
            except Exception as e:
                print(f"[TCP_READER] Exception: {e}")
                break

        print("[TCP_READER] Exiting thread.")


    def _send_h264_data(self):
        """
        Continuously reads binary data from a TCP socket and appends it to a shared buffer in a thread-safe way.

        Parameters:
        - tcp_socket (socket.socket): The connected TCP socket to read binary data from.

        Shared Resources (instance attributes):
        - h264_buffer (collections.deque): Thread-safe buffer to store received data chunks.
        - buffer_lock (threading.Lock): Used to synchronize access to the buffer.
        - running (threading.Event): Controls when reading should continue or stop.
        - chunk_size (int): Number of bytes to read from the socket at once.

        Behavior:
        - Continuously reads from the socket while `running` is set.
        - Appends each chunk to the buffer with proper thread synchronization.
        - Stops gracefully on socket closure or exception.
        """

        if self.udp_socket is None:
            self.udp_socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
            print(f"[{threading.current_thread().name}] UDP socket created: {self.udp_ip}:{self.udp_port}")   

        try:
            while self.running.is_set():
                chunk = None
                with self.buffer_lock:
                    if self.h264_buffer:
                        chunk = self.h264_buffer.popleft()

                if chunk:
                    self.udp_socket.sendto(chunk, (self.udp_ip, self.udp_port))

            # Small sleep to avoid 100% CPU usage when buffer is empty

        except Exception as e:
            print(f"[{threading.current_thread().name}] Błąd podczas wysyłania UDP: {e}")
        finally:
            end_time = time.time()
            duration = end_time - start_time
            print(f"[{threading.current_thread().name}] Sending stopped. Total sent: {bytes_sent_count} bytes in {duration:.2f}s.")            
            
            # Close UDP connection
            if self.udp_socket:
                self.udp_socket.close()
                self.udp_socket = None # Oznacza socket jako zamknięty
                print(f"[{threading.current_thread().name}] Socket UDP zamknięty.")



    def start(self):
        """Starts TCP reader and UDP sender threads."""
        print(f"[*] Connecting to scrcpy TCP stream at {self.tcp_host}:{self.tcp_port}...")
        
        try:
            # Create and configure TCP socket
            self.tcp_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            self.tcp_socket.connect((self.tcp_host, self.tcp_port))
            
            # Start TCP reader thread
            self.tcp_reader_thread = threading.Thread(
                target=self._read_tcp_to_buffer,
                # args=(),
                daemon=False,
                name="TCPReaderThread"
            )
            self.tcp_reader_thread.start()

            
            # Start UDP sender thread
            self.sender_thread = threading.Thread(
                target=self._send_h264_data,
                daemon=False,
                name="UDPSenderThread"
            )
            self.sender_thread.start()

            return True
            
        except Exception as e:
            print(f"[!!!] Connection failed: {e}")
            self.stop()
            return False

    def stop(self):
        print("\n[*] Stopping all processes and threads...")


