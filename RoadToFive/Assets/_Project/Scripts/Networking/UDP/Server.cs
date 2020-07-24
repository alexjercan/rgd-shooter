using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using _Project.Scripts.Networking.ByteArray;

namespace _Project.Scripts.Networking.UDP
{
    public class Server
    {
        public event EventHandler<ByteArrayReader> ReceivedDatagram;
        
        private readonly Dictionary<int, IPEndPoint> _knownHosts = new Dictionary<int, IPEndPoint>();
        private readonly UserDatagramProtocolServer _udpServer;

        public IPEndPoint GetHost(int id) => _knownHosts[id];

        public Server(int port)
        {
            _udpServer = new UserDatagramProtocolServer(new IPEndPoint(IPAddress.Any, port));
        }

        public void Listen()
        {
            _udpServer.Listen(ReceiveCallback);
        }
        
        public void SendDatagram(byte[] datagram, IPEndPoint remoteHost) => _udpServer.SendDatagram(datagram, remoteHost);

        public void BroadcastDatagram(byte[] datagram)
        {
            foreach (var knownHost in _knownHosts.Values) 
                SendDatagram(datagram, knownHost);
        }
        
        public void BroadcastDatagramExcept(int clientId, byte[] data)
        {
            foreach (var idEndPointPairs in _knownHosts.Where(idEndPointPairs => clientId != idEndPointPairs.Key))
                SendDatagram(data, idEndPointPairs.Value);
        }
        
        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            var socket = (UdpClient) asyncResult.AsyncState;
            var clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            var receivedBytes = socket.EndReceive(asyncResult, ref clientEndPoint);
            
            socket.BeginReceive(ReceiveCallback, socket);
            
            if (receivedBytes.Length < 4) return;
            
            var receiveDatagram = new ByteArrayReader(receivedBytes);
            var clientId = receiveDatagram.ReadInt();
            
            var datagramLength = receiveDatagram.ReadInt();
            if (datagramLength != receiveDatagram.UnreadBytes) return;

            if (clientId == 0)
            {
                if (_knownHosts.ContainsValue(clientEndPoint)) return;

                var type = receiveDatagram.ReadInt();
                if (type != (int)MessageType.Dummy) return;
                
                var newClientId = MessageTemplates.ReadDummy(receiveDatagram);
                if (newClientId == 0) newClientId = _knownHosts.Count + 1;
                if (_knownHosts.ContainsKey(newClientId)) _knownHosts[newClientId] = clientEndPoint;
                else _knownHosts.Add(newClientId, clientEndPoint);
                SendDatagram(MessageTemplates.WriteDummy(newClientId), clientEndPoint);
                return;
            }
            
            if (_knownHosts[clientId].ToString() != clientEndPoint.ToString()) return;

            OnReceivedDatagram(receiveDatagram);
        }

        private void OnReceivedDatagram(ByteArrayReader e) => ReceivedDatagram?.Invoke(this, e);
    }
}