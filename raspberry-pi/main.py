import asyncio
from src.SerialConnection import SerialConnection
from src.WebSocketsClient import websocket_client

WS_URI = "ws://localhost:8181"

def main():
    try:
        with SerialConnection('/dev/ttyACM0', 9600, timeout=1) as serial_conn:

            response = serial_conn.read_line()
            if response:
                print("Initial response from Arduino:", response)

            # Run the async WebSocket client, passing serial connection instance
            asyncio.run(websocket_client(WS_URI, serial_conn))

    except Exception as e:
        print(f"Error in main: {e}")

if __name__ == "__main__":
    main()
