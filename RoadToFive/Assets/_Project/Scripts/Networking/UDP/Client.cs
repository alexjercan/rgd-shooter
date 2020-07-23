using System;
using System.Net;
using System.Net.Sockets;
using _Project.Scripts.Logging;
using _Project.Scripts.Networking.ByteArray;
using _Project.Scripts.Networking.Datagram;
using _Project.Scripts.Threading;

namespace _Project.Scripts.Networking.UDP
{
    public class Client
    {
        private int _id;
        private readonly IUserDatagramProtocolSocket _udpClient;
        
        public Client(string remoteIp, int remotePort, int localPort)
        {
            _id = 0;
            _udpClient = new UserDatagramProtocolClient(new IPEndPoint(IPAddress.Any, localPort),
                new IPEndPoint(IPAddress.Parse(remoteIp), remotePort));
            
            _udpClient.Listen(ReceiveCallback);
        }

        public void SendDatagram(byte[] datagram)
        {
            var byteArrayBuilder = new ByteArrayBuilder().Write(_id).Write(datagram).ToByteArray();
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
            
            MainThreadScheduler.EnqueueOnMainThread(() => ReadDatagram(receiveDatagram));
        }
        
        private void ReadDatagram(ByteArrayReader receiveDatagram)
        {

            var datagramType = (ServerDatagram)receiveDatagram.ReadInt();

            switch (datagramType)
            {
                case ServerDatagram.InvalidDatagram:
                    break;
                case ServerDatagram.WelcomeDatagram:
                    HandleWelcomeDatagram(receiveDatagram);
                    break;
                default:
                    return;
            }
        }
        
        private void HandleWelcomeDatagram(ByteArrayReader receiveDatagram)
        {
            _id = DatagramTemplates.ReadWelcomeMessage(receiveDatagram);
            Logger.Info(_id.ToString());
            
            SendDatagram(DatagramTemplates.WriteWelcomeReceivedMessage("guest " + _id));
        }
    }
}