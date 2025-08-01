using Fleck;
using System;
using System.Collections.Generic;

namespace FleckWebSocketApp.WebSockets
{
    public class WebSocketServerManager
    {
        private List<IWebSocketConnection> allSockets = new();

        public void Start(string url = "ws://0.0.0.0:8181")
        {
            var server = new WebSocketServer(url);
            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    Console.WriteLine("Client connected");
                    allSockets.Add(socket);
                };
                socket.OnClose = () =>
                {
                    Console.WriteLine("Client disconnected");
                    allSockets.Remove(socket);
                };
                socket.OnMessage = message =>
                {
                    Console.WriteLine("Received: " + message);
                    foreach (var s in allSockets)
                    {
                        s.Send("Echo: " + message);
                    }
                };
            });

            Console.WriteLine($"WebSocket server started on {url}");
        }
    }
}
