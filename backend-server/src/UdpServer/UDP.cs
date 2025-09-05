using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDP
{
    public class UDPSocket
    {
        private Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private const int bufSize = 1400;
        private EndPoint epFrom = new IPEndPoint(IPAddress.Any, 0);
        private AsyncCallback recv = null;
        public byte[] buffer = new byte[bufSize];
        int bytesInBuffer = 0;

        public void Server(string address, int port)
        {
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
            _socket.Bind(new IPEndPoint(IPAddress.Parse(address), port));

            Console.WriteLine("Server");

            StartReceiveingData();

        }

        private void StartReceiveingData()
        {

            if (bytesInBuffer >= 1400)
            {
                Console.WriteLine("Buffer is full, stopping receive.");
                _socket.Close();
                return;
            }


            try
            {
                IPEndPoint ipeSender = new IPEndPoint(IPAddress.Any, 0);
                EndPoint epSender = (EndPoint)ipeSender;

                _socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epSender, new AsyncCallback(OnReceive), epSender);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void OnReceive(IAsyncResult ar)
        {
            try
            {

                IPEndPoint ipeSender = new IPEndPoint(IPAddress.Any, 0);
                EndPoint epSender = (EndPoint)ipeSender;
                int numBytes = _socket.EndReceiveFrom(ar, ref epSender);

                using (FileStream fs = new FileStream("receivedData.bin", FileMode.Append, FileAccess.Write))
                {
                    fs.Write(buffer, 0, numBytes);
                }

                bytesInBuffer++;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            StartReceiveingData();
        }
    }
}