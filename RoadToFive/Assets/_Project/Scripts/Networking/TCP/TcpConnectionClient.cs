using System;
using System.Net.Sockets;

namespace _Project.Scripts.Networking.TCP
{
    public class TcpConnectionClient : TcpConnection
    {
        //This class represents a new tcp connection on the client side.
        //It exposes a socket that the client uses to communicate with the server.

        private readonly string _ip;
        private readonly int _port;

        public TcpConnectionClient(string ip, int port)
        {
            _ip = ip;
            _port = port;
        }

        public override void Connect(TcpClient socket)
        {
            Socket = socket;
            Socket.ReceiveBufferSize = DataBufferSize;
            Socket.SendBufferSize = DataBufferSize;

            ReceiveBuffer = new byte[DataBufferSize];
            Socket.BeginConnect(_ip, _port, ConnectCallback, Socket);
        }

        private void ConnectCallback(IAsyncResult asyncResult)
        {
            Socket.EndConnect(asyncResult);

            if (!Socket.Connected) return;

            NetworkStream = Socket.GetStream();
            NetworkStream.BeginRead(ReceiveBuffer, 0, DataBufferSize, ReceiveCallback, null);
        }
    }
}