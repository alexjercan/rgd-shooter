using System;
using System.Net;
using System.Net.Sockets;
using _Project.Scripts.Networking.ByteArray;

namespace _Project.Scripts.Networking.UDP
{
    public class Client
    {
        public int Id { get; set; }

        public event EventHandler<ByteArrayReader> ReceivedDatagram;

        private readonly IUserDatagramProtocolSocket _udpClient;
        
        public Client(string remoteIp, int remotePort, int localPort)
        {
            Id = 0;
            _udpClient = new UserDatagramProtocolClient(new IPEndPoint(IPAddress.Any, localPort),
                new IPEndPoint(IPAddress.Parse(remoteIp), remotePort));
        }

        public void Listen()
        {
            _udpClient.Listen(ReceiveCallback);
        }

        public void SendDatagram(byte[] datagram)
        {
            var byteArrayBuilder = new ByteArrayBuilder().Write(Id).Write(datagram).ToByteArray();
            _udpClient.SendDatagram(byteArrayBuilder, null);
        }
        
        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            var socket = (UdpClient) asyncResult.AsyncState;
            var remoteHost = new IPEndPoint(IPAddress.Any, 0);
            var receivedBytes = socket.EndReceive(asyncResult, ref remoteHost);
            
            socket.BeginReceive(ReceiveCallback, socket);
            
            if (receivedBytes.Length <= 0) return;
            
            var receiveDatagram = new ByteArrayReader(receivedBytes);
            var datagramLength = receiveDatagram.ReadInt();
            if (datagramLength != receiveDatagram.UnreadBytes) return;

            OnReceivedDatagram(receiveDatagram);
        }

        private void OnReceivedDatagram(ByteArrayReader e) => ReceivedDatagram?.Invoke(this, e);
    }
}