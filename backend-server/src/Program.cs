using System;

using FleckWebSocketApp.WebSockets;

class Program
{
    static void Main(string[] args)
    {
        var server = new WebSocketServerManager();
        server.Start();

        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
    }
}
