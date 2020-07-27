using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Networking.ClientSide
{
    public class Client
    {
        public static string ServerIp { get; set; }
        public static int ServerPort { get; set; }
        public static int MyId { get; set; }
        public static ServerConnection Connection { get; set; }

        public delegate void PacketHandler(Packet packet);
        public static Dictionary<int, PacketHandler> PacketHandlers;
        public static void ConnectToServer(string ip)
        {
            Debug.Log($"Connecting to server {ip}");
            ServerPort = 26950;
            ServerIp = ip;
            Connection = new ServerConnection();
            InitializeClientData();
            Connection.Tcp.Connect();
        }
        
        private static void InitializeClientData()
        {
            PacketHandlers = new Dictionary<int, PacketHandler>()
            {
                {(int)ServerPackets.Welcome, ClientHandle.Welcome},
            };
        }
    }
}