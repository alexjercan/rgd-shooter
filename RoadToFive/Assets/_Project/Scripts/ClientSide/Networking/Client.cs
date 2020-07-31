using UnityEngine;

namespace _Project.Scripts.ClientSide.Networking
{
    public static class Client  //DONE DO NOT MODIFY
    {
        public static string ServerIp { get; private set; }
        public static int ServerPort { get; private set; }
        public static int MyId { get; set; }
        public static ServerConnection Connection { get; private set; }

        private static bool _isConnected;
        
        public static void ConnectToServer(string ip)
        {
            Debug.Log($"Connecting to server {ip}");
            _isConnected = true;
            ServerPort = 26950;
            ServerIp = ip;
            Connection = new ServerConnection();
            ClientHandle.InitializeClientData();
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
    }
}