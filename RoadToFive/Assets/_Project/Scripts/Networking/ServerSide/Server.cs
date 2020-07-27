using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace _Project.Scripts.Networking.ServerSide
{
    public static class Server
    {
        public static int MaxPlayers { get; private set; }
        private static int Port { get; set; }
        
        public static readonly Dictionary<int, ClientConnection> ClientConnections = new Dictionary<int, ClientConnection>();

        public delegate void PacketHandler(int fromClient, Packet packet);
        public static Dictionary<int, PacketHandler> PacketHandlers;

        private static TcpListener TcpListener { get; set; }
        public static UdpClient UdpListener { get; private set; }

        public static void Start(int maxPlayers, int port)
        {
            MaxPlayers = maxPlayers;
            Port = port;
            
            InitializeServerData();
            
            TcpListener = new TcpListener(IPAddress.Any, port);
            TcpListener.Start();
            TcpListener.BeginAcceptTcpClient(TcpConnectCallback, null);
            
            UdpListener = new UdpClient(Port);
            UdpListener.BeginReceive(UdpReceiveCallback, null);
            
            Debug.Log($"Server started on port {Port}");
        }

        private static void TcpConnectCallback(IAsyncResult result)
        {
            var client = TcpListener.EndAcceptTcpClient(result);
            TcpListener.BeginAcceptTcpClient(TcpConnectCallback, null);
            
            Debug.Log($"Incoming connection from {client.Client.RemoteEndPoint}...");
            
            for (var i = 1; i <= MaxPlayers; i++)
            {
                if (ClientConnections[i].Tcp.Socket != null) continue;
                ClientConnections[i].Tcp.Connect(client);
                return;
            }
        }
        
        private static void UdpReceiveCallback(IAsyncResult result)
        {
            try
            {
                var clientIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                var data = UdpListener.EndReceive(result, ref clientIpEndPoint);
                UdpListener.BeginReceive(UdpReceiveCallback, null);

                if (data.Length < 4) return;

                using (var packet = new Packet(data))
                {
                    var clientId = packet.ReadInt();

                    if (clientId == 0) return;

                    if (ClientConnections[clientId].Udp.ClientEndPoint == null)
                    {
                        ClientConnections[clientId].Udp.Connect(clientIpEndPoint);
                        return;
                    }

                    if (ClientConnections[clientId].Udp.ClientEndPoint.ToString() != clientIpEndPoint.ToString()) return;

                    ClientConnections[clientId].Udp.HandleData(packet);
                }
            }
            catch (Exception e)
            {
                Debug.Log($"Error receiving UDP data: {e}");
            }
        }
        
        private static void InitializeServerData()
        {
            for (var i = 1; i <= MaxPlayers; i++) ClientConnections.Add(i, new ClientConnection(i));
            PacketHandlers = new Dictionary<int, PacketHandler>()
            {
                {(int) ClientPackets.WelcomeReceived, ServerHandle.WelcomeReceived},
                {(int) ClientPackets.PlayerMovement, ServerHandle.PlayerMovement},
            };
        }
    }
}