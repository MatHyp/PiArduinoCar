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