using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using _Project.Scripts.Networking.ByteArray;

namespace _Project.Scripts.Networking.TCP
{
    public class Server
    {
        private readonly int _maxPlayerCount;
        private readonly int _port;

        private readonly Dictionary<int, ServerSocket> _sockets = new Dictionary<int, ServerSocket>();
        private readonly TcpListener _tcpListener;

        public Server(int maxPlayerCount, int port)
        {
            _maxPlayerCount = maxPlayerCount;
            _port = port;

            for (var i = 1; i <= _maxPlayerCount; i++) _sockets.Add(i, new ServerSocket());

            _tcpListener = new TcpListener(IPAddress.Any, _port);
        }

        public void Listen()
        {
            _tcpListener.Start();
            _tcpListener.BeginAcceptTcpClient(TcpConnectCallback, null);
            Console.WriteLine($"Server started on port {_port}");
        }
        
        public void SendPacket(int client, byte[] data) => _sockets[client].SendPacket(data);

        public void BroadcastPacket(byte[] data)
        {
            foreach (var connection in _sockets.Values ) 
                connection.SendPacket(data);
        }
        
        public void BroadcastPacketExcept(int clientId, byte[] data)
        {
            foreach (var idConnectionPairs in _sockets.Where(idConnectionPairs => clientId != idConnectionPairs.Key))
                idConnectionPairs.Value.SendPacket(data);
        }

        public void Disconnect()
        {
            _tcpListener.Stop();
            for (var i = 1; i <= _maxPlayerCount; i++) _sockets[i].Disconnect();
        }

        private void TcpConnectCallback(IAsyncResult asyncResult)
        {
            var client = _tcpListener.EndAcceptTcpClient(asyncResult);
            _tcpListener.BeginAcceptTcpClient(TcpConnectCallback, null);
            Console.Write($"Incoming connection from {client.Client.RemoteEndPoint}...");

            for (var i = 1; i <= _maxPlayerCount; i++)
            {
                if (_sockets[i].Socket != null) continue;
                
                Console.WriteLine($" connecting it on socket {i}");
                _sockets[i].Connect(client);
                SendPacket(i, MessageTemplates.WriteWelcome(i, "welcome 69"));
                return;
            }
            
            Console.WriteLine(" failed to connect: Server full!");
        }

        public void SetReceivedPacketHandler(ReceivedHandler handler)
        {
            foreach (var socket in _sockets.Values)
                socket.ReceivedPacket += (sender, reader) => handler(sender, reader);
        }
        
        public delegate void ReceivedHandler(object sender, ByteArrayReader reader);
    }
}