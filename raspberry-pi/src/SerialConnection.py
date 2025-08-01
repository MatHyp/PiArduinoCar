import serial
import time

class SerialConnection:
    def __init__(self, port='/dev/ttyACM0', baudrate=9600, timeout=1):
        self.port = port
        self.baudrate = baudrate
        self.timeout = timeout
        self.ser = None

    def open(self):
        try:
            self.ser = serial.Serial(self.port, self.baudrate, timeout=self.timeout)
            time.sleep(2)
            print(f"Serial port {self.port} opened")
        except serial.SerialException as e:
            print(f"Serial error: {e}")
            self.ser = None

    def write(self, data: bytes):
        if self.ser and self.ser.is_open:
            self.ser.write(data)
        else:
            print("Serial port not open")

    def read_line(self):
        if self.ser and self.ser.is_open:
            try:
                line = self.ser.readline().decode('utf-8').rstrip()
                return line if line else None
            except Exception as e:
                print(f"Serial read error: {e}")
                return None
        return None

    def close(self):
        if self.ser and self.ser.is_open:
            self.ser.close()
            print("Serial port closed")

    def __enter__(self):
        self.open()
        return self

    def __exit__(self, exc_type, exc_val, exc_tb):
        self.close()
