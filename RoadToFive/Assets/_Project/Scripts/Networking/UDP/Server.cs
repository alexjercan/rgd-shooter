using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using _Project.Scripts.Logging;
using _Project.Scripts.Networking.ByteArray;
using _Project.Scripts.Networking.Datagram;
using _Project.Scripts.Threading;

namespace _Project.Scripts.Networking.UDP
{
    public class Server
    {
        private readonly Dictionary<int, IPEndPoint> _knownHosts = new Dictionary<int, IPEndPoint>();
        private readonly UserDatagramProtocolServer _udpServer;
        
        public Server(int port)
        {
            _udpServer = new UserDatagramProtocolServer(new IPEndPoint(IPAddress.Any, port));
            
            _udpServer.Listen(ReceiveCallback);
        }
        
        public void SendDatagram(byte[] datagram, IPEndPoint remoteHost) => _udpServer.SendDatagram(datagram, remoteHost);

        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            var socket = (UdpClient) asyncResult.AsyncState;
            var clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            var receivedBytes = socket.EndReceive(asyncResult, ref clientEndPoint);
            
            socket.BeginReceive(ReceiveCallback, socket);
            
            if (receivedBytes.Length < 4) return;
            
            var receiveDatagram = new ByteArrayReader(receivedBytes);
            var clientId = receiveDatagram.ReadInt();

            if (clientId == 0)
            {
                _knownHosts.Add(_knownHosts.Count, clientEndPoint);
                SendDatagram(DatagramTemplates.WriteWelcomeMessage(_knownHosts.Count), clientEndPoint);
                return;
            }
            
            var datagramLength = receiveDatagram.ReadInt();
            if (datagramLength != receiveDatagram.UnreadBytes) return;
            
            MainThreadScheduler.EnqueueOnMainThread(() => ReadDatagram(receiveDatagram));
        }
        
        private void ReadDatagram(ByteArrayReader receiveDatagram)
        {

            var datagramType = (ClientDatagram)receiveDatagram.ReadInt();

            switch (datagramType)
            {
                case ClientDatagram.InvalidDatagram:
                    break;
                case ClientDatagram.WelcomeReceived:
                    HandleWelcomeReceived(receiveDatagram);
                    break;
                default:
                    return;
            }
        }
        
        private void HandleWelcomeReceived(ByteArrayReader receiveDatagram)
        {
            var message = DatagramTemplates.ReadWelcomeReceivedMessage(receiveDatagram);
            
            Logger.Info(message);
        }
    }
}