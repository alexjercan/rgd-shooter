using System;
using System.Net;
using System.Net.Sockets;
using _Project.Scripts.Networking.ByteArray;
using _Project.Scripts.Threading;

namespace _Project.Scripts.Networking.UDP
{
    class UdpBindingClient : UdpBinding
    {
        public override IPEndPoint EndPoint => _endPoint;
        
        private readonly ClientUdp _clientUdp;
        private IPEndPoint _endPoint;

        public UdpBindingClient(ClientUdp clientUdp, string serverIp, int port)
        {
            _clientUdp = clientUdp;
            _endPoint = new IPEndPoint(IPAddress.Parse(serverIp), port);
        }
        
        public override void Bind(IPEndPoint endPoint)
        {
            Socket = new UdpClient(endPoint.Port);
            
            Socket.Connect(_endPoint);
            Socket.BeginReceive(ReceiveCallback, null);
        }

        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            var data = Socket.EndReceive(asyncResult, ref _endPoint);
            Socket.BeginReceive(ReceiveCallback, null);
            if (data.Length <= 0) return;
            
            var receivedPacketReader = new ByteArrayReader(data);
            var packetLength = receivedPacketReader.ReadInt();

            var packetBytes = receivedPacketReader.ReadBytes(packetLength);
            MainThreadScheduler.EnqueueOnMainThread(() => _clientUdp.ReadDatagram(packetBytes));
        }
    }
}