using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using _Project.Scripts.Logging;
using _Project.Scripts.Networking.Packet;
using _Project.Scripts.Networking.TCP;

namespace _Project.Scripts.Networking
{
    public class Server
    {
        public int MaxPlayerCount { get; set; }
        public int Port { get; set; }

        private readonly Dictionary<int, TcpConnection> _connections = new Dictionary<int, TcpConnection>();
        private readonly TcpListener _tcpListener;

        public Server(int maxPlayerCount, int port)
        {
            MaxPlayerCount = maxPlayerCount;
            Port = port;
            
            InitializeServerData();
            
            _tcpListener = new TcpListener(IPAddress.Any, Port);
        }

        public void Start()
        {
            _tcpListener.Start();
            _tcpListener.BeginAcceptTcpClient(TcpConnectCallback, null);
            Logger.Info($"Server started on port {Port}");
        }

        public void SendPacket(int client, byte[] data)
        {
            _connections[client].SendPacket(data);
        }

        public void BroadcastPacket(byte[] data)
        {
            foreach (var connection in _connections.Values )
            {
                connection.SendPacket(data);
            }
        }
        
        private void TcpConnectCallback(IAsyncResult asyncResult)
        {
            var client = _tcpListener.EndAcceptTcpClient(asyncResult);
            _tcpListener.BeginAcceptTcpClient(TcpConnectCallback, null);
            Logger.Info($"Incoming connection from {client.Client.RemoteEndPoint}...");

            for (var i = 1; i <= MaxPlayerCount; i++)
            {
                if (_connections[i].Socket != null) continue;
                
                _connections[i].Connect(client);
                SendPacket(i, new WelcomePacketWriter(i, 69).WritePacket());
                return;
            }
            
            Logger.Warning($"{client.Client.RemoteEndPoint} failed to connect: Server full!");
        }

        private void InitializeServerData()
        {
            for (var i = 1; i <= MaxPlayerCount; i++) _connections.Add(i, new TcpConnectionServer());
        }
    }
}