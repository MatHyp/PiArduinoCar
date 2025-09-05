import random
from src.UDPServer import UDPServer
from src.SerialConnection import SerialConnection
from src.ScrcpyScreenCapture import ScrcpyScreenCapture

import subprocess
import threading
import time
import sys
import os
import socket 
from collections import deque
def main():
    # with SerialConnection(port="/dev/ttyACM0", baudrate=9600) as serial_conn:
    #     udp_receiver = UDPServer(udp_ip="0.0.0.0", udp_port=12000)
    #     try:
    #         while True:
    #             # Check for UDP data
    #             data = udp_receiver.receive()
    #             if data:
    #                 serial_conn.write(data)
    #                 print(f"Sent to Arduino: {data}")


    #     except KeyboardInterrupt:
    #         print("Exiting program...")
    #     finally:
    #         udp_receiver.close()
    # capture_system = ScrcpyScreenCapture()
    # capture_system.start()
    
    tcp_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    tcp_socket.connect(("127.0.0.1", 1234))

    tcp_socket_2 = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    tcp_socket_2.connect(("127.0.0.1", 12345))

    
    while True:
        data = tcp_socket.recv(4096)  # receive data
        if not data:
            break  # Connection closed
        tcp_socket_2.sendall(data)



if __name__ == "__main__":
    main()