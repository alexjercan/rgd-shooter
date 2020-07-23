using System;
using System.Net.Sockets;

namespace _Project.Scripts.Networking.TCP
{
    public class TransmissionControlProtocolClient : TransmissionControlProtocolSocket
    {
        private readonly Client _client;
        private readonly string _ipServer;
        private readonly int _port;

        public TransmissionControlProtocolClient(Client client, string ipServer, int port)
        {
            _ipServer = ipServer;
            _port = port;
            _client = client;
        }

        public override void Connect(TcpClient socket)
        {
            Socket = socket;
            Socket.ReceiveBufferSize = DataBufferSize;
            Socket.SendBufferSize = DataBufferSize;

            ReceivedBuffer = new byte[DataBufferSize];
            Socket.BeginConnect(_ipServer, _port, ConnectCallback, Socket);
        }

        protected override void HandleReadPacket(byte[] packet) => _client.ReadPacket(packet);

        private void ConnectCallback(IAsyncResult asyncResult)
        {
            Socket.EndConnect(asyncResult);

            if (!Socket.Connected) return;

            NetworkStream = Socket.GetStream();
            NetworkStream.BeginRead(ReceivedBuffer, 0, DataBufferSize, ReceiveCallback, null);
        }
    }
}