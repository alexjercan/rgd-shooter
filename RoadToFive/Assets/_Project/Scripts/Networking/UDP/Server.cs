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
        /// <summary>
        /// This event is being invoked when a message is received by the socket.
        /// </summary>
        public event EventHandler<ByteArrayReader> ReceivedDatagram;
        
        private readonly Dictionary<int, IPEndPoint> _knownHosts = new Dictionary<int, IPEndPoint>();
        private readonly ServerSocket _socket;

        /// <summary>
        /// Creates a new socket and binds it to the local port.
        /// </summary>
        /// <param name="port"></param>
        public Server(int port) => _socket = new ServerSocket(new IPEndPoint(IPAddress.Any, port));

        /// <summary>
        /// Start listening on the socket for datagrams from the clients.
        /// </summary>
        public void Listen() => _socket.Listen(ReceiveCallback);

        /// <summary>
        /// Sends a datagram to the specified remote host.
        /// </summary>
        /// <param name="datagram"></param>
        /// <param name="hostId"></param>
        public void SendDatagram(byte[] datagram, int hostId) => _socket.SendDatagram(datagram, _knownHosts[hostId]);

        /// <summary>
        /// Broadcasts a datagram to all known hosts
        /// </summary>
        /// <param name="datagram"></param>
        public void BroadcastDatagram(byte[] datagram)
        {
            foreach (var hostId in _knownHosts.Keys) 
                SendDatagram(datagram, hostId);
        }
        
        /// <summary>
        /// Broadcasts a datagram to all known hosts except one.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="data"></param>
        public void BroadcastDatagramExcept(int clientId, byte[] data)
        {
            foreach (var idEndPointPairs in _knownHosts.Where(idEndPointPairs => clientId != idEndPointPairs.Key))
                SendDatagram(data, idEndPointPairs.Key);
        }
        
        public void Disconnect()
        {
            _socket.Disconnect();
            _knownHosts.Clear();
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
                SendDatagram(MessageTemplates.WriteDummy(newClientId), newClientId);
                return;
            }
            
            if (_knownHosts[clientId].ToString() != clientEndPoint.ToString()) return;

            OnReceivedDatagram(receiveDatagram);
        }

        private void OnReceivedDatagram(ByteArrayReader e) => ReceivedDatagram?.Invoke(this, e);
    }
}