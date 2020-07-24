﻿using System;
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
        private readonly int _maxPlayerCount;
        private readonly int _port;

        private readonly Dictionary<int, TransmissionControlProtocolSocket> _sockets = new Dictionary<int, TransmissionControlProtocolSocket>();
        private readonly TcpListener _tcpListener;

        public Server(int maxPlayerCount, int port)
        {
            _maxPlayerCount = maxPlayerCount;
            _port = port;

            for (var i = 1; i <= _maxPlayerCount; i++) _sockets.Add(i, new TransmissionControlProtocolServer());

            _tcpListener = new TcpListener(IPAddress.Any, _port);
        }

        public void Listen()
        {
            _tcpListener.Start();
            _tcpListener.BeginAcceptTcpClient(TcpConnectCallback, null);
            Logger.Info($"Server started on port {_port}");
        }
        
        public void SendPacket(int client, byte[] data) => _sockets[client].SendPacket(data);

        public void BroadcastPacket(byte[] data)
        {
            foreach (var connection in _sockets.Values ) connection.SendPacket(data);
        }
        
        private void TcpConnectCallback(IAsyncResult asyncResult)
        {
            var client = _tcpListener.EndAcceptTcpClient(asyncResult);
            _tcpListener.BeginAcceptTcpClient(TcpConnectCallback, null);
            Logger.Info($"Incoming connection from {client.Client.RemoteEndPoint}...");

            for (var i = 1; i <= _maxPlayerCount; i++)
            {
                if (_sockets[i].Socket != null) continue;
                
                _sockets[i].Connect(client);
                SendPacket(i, PacketTemplates.WriteWelcomePacket(i, "welcome 69"));
                return;
            }
            
            Logger.Warning($"{client.Client.RemoteEndPoint} failed to connect: Server full!");
        }

        public void SetReceivedDatagramHandler(ReceivedHandler handler)
        {
            foreach (var socket in _sockets.Values)
                socket.ReceivedDatagram += (sender, reader) => handler(sender, reader);
        }
        
        public delegate void ReceivedHandler(object sender, ByteArrayReader reader);
    }
}