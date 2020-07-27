using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Networking.ClientSide
{
    public class Client
    {
        public static string ServerIp { get; private set; }
        public static int ServerPort { get; private set; }
        public static int MyId { get; set; }
        public static ServerConnection Connection { get; private set; }

        public delegate void PacketHandler(Packet packet);
        public static Dictionary<int, PacketHandler> PacketHandlers;
        
        private static bool _isConnected;
        
        public static void ConnectToServer(string ip)
        {
            Debug.Log($"Connecting to server {ip}");
            _isConnected = true;
            ServerPort = 26950;
            ServerIp = ip;
            Connection = new ServerConnection();
            InitializeClientData();
            Connection.Tcp.Connect();
        }

        public static void Disconnect()
        {
            if (!_isConnected) return;

            _isConnected = false;
            Connection.Tcp.Socket.Close();
            Connection.Udp.Socket.Close();
            
            Debug.Log("Disconnected from the server.");
        }
        
        private static void InitializeClientData()
        {
            PacketHandlers = new Dictionary<int, PacketHandler>()
            {
                {(int)ServerPackets.Welcome, ClientHandle.Welcome},
                {(int)ServerPackets.SpawnPlayer, ClientHandle.SpawnPlayer},
                {(int)ServerPackets.PlayerPosition, ClientHandle.PlayerPosition},
                {(int)ServerPackets.PlayerRotation, ClientHandle.PlayerRotation},
                
            };
        }
    }
}