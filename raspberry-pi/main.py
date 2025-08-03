import asyncio
from src.SerialConnection import SerialConnection
from src.WebSocketsClient import websocket_client
from src.ScrcpyScreenCapture import ScrcpyScreenCapture

import socket # Dodane dla faktycznego wysyłania UDP


def main():


    capture_system = ScrcpyScreenCapture()
    capture_system.start()
   


if __name__ == "__main__":
    main()