import random
from src.UDPServer import UDPServer
from src.SerialConnection import SerialConnection


def main():
    with SerialConnection(port="/dev/ttyACM0", baudrate=9600) as serial_conn:
        udp_receiver = UDPServer(udp_ip="0.0.0.0", udp_port=12000)
        try:
            while True:
                # Check for UDP data
                data = udp_receiver.receive()
                if data:
                    serial_conn.write(data)
                    print(f"Sent to Arduino: {data}")


        except KeyboardInterrupt:
            print("Exiting program...")
        finally:
            udp_receiver.close()

if __name__ == "__main__":
    main()