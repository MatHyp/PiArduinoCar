import asyncio
import websockets

async def websocket_client(uri, serial_conn):
    try:
        async with websockets.connect(uri) as ws:
            print(f"Connected to WS server {uri}")
            
    except Exception as e:
        print(f"WebSocket error: {e}")