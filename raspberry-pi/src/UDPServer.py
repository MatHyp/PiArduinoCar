import socket
import struct
import binascii

class UDPServer:
    def __init__(self, udp_ip="127.0.0.1", udp_port=12000, buffer_size=1):
        self.udp_ip = udp_ip
        self.udp_port = udp_port
        self.buffer_size = buffer_size
        self.sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        self.sock.bind((self.udp_ip, self.udp_port))
        print(f"UDP Receiver listening on {self.udp_ip}:{self.udp_port}")

    def receive(self):
        try:
            data, addr = self.sock.recvfrom(self.buffer_size)
            print(f"Received {data} from {addr}")
            return data
        except Exception as e:
            print(f"UDP receive error: {e}")
            return None

    def close(self):
        self.sock.close()
        print("UDP socket closed")