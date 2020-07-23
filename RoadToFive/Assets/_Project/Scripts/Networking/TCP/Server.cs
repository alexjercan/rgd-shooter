using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using _Project.Scripts.Logging;
using _Project.Scripts.Networking.ByteArray;
using _Project.Scripts.Networking.Packet;

namespace _Project.Scripts.Networking.TCP
{
    public class Server
    {
        private int MaxPlayerCount { get; set; }
        private int Port { get; set; }

        private readonly Dictionary<int, TransmissionControlProtocolSocket> _connections = new Dictionary<int, TransmissionControlProtocolSocket>();
        private readonly TcpListener _tcpListener;

        public Server(int maxPlayerCount, int port)
        {
            MaxPlayerCount = maxPlayerCount;
            Port = port;
            
            for (var i = 1; i <= MaxPlayerCount; i++) _connections.Add(i, new TransmissionControlProtocolServer(this));
            
            _tcpListener = new TcpListener(IPAddress.Any, Port);
            
            _tcpListener.Start();
            _tcpListener.BeginAcceptTcpClient(TcpConnectCallback, null);
            Logger.Info($"Server started on port {Port}");
        }

        public void SendPacket(int client, byte[] data) => _connections[client].SendPacket(data);

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
                SendPacket(i, PacketTemplates.WriteWelcomePacket(i, "welcome 69"));
                return;
            }
            
            Logger.Warning($"{client.Client.RemoteEndPoint} failed to connect: Server full!");
        }

        public void ReadPacket(byte[] packet)
        {
            var packetReader = new ByteArrayReader(packet);
            var packetType = (ClientPacket)packetReader.ReadInt();

            switch (packetType)
            {
                case ClientPacket.InvalidPacket:
                    break;
                case ClientPacket.WelcomeReceived:
                    HandleWelcomeReceived(packetReader);
                    break;
                default:
                    return;
            }
        }
        
        private void HandleWelcomeReceived(ByteArrayReader byteArrayReader)
        {
            var (_, message) = PacketTemplates.WriteWelcomeReceivedPacket(byteArrayReader);
            
            Logger.Info(message);
        }
    }
}