using System;

using FleckWebSocketApp.WebSockets;
using UDP; // Użyj przestrzeni nazw z udp.cs
class Program
{
    static void Main(string[] args)
    {

        UDPSocket udpServer = new UDPSocket();
        udpServer.Server("127.0.0.1", 27000);



        Console.ReadKey();
    }
}
