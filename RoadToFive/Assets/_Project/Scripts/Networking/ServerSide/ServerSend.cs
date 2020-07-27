using UnityEngine;

namespace _Project.Scripts.Networking.ServerSide
{
    public class ServerSend
    {
        public static void Welcome(int toClient, string message)
        {
            using (var packet = new Packet((int) ServerPackets.Welcome))
            {
                packet.Write(message).Write(toClient);
Debug.Log("Sending welcome");
                SendTcpData(toClient, packet);
            }
        }

        private static void SendTcpData(int toClient, Packet packet)
        {
            packet.InsertLength();
            Server.Sockets[toClient].TcpSendData(packet);
        }

        private static void SendTcpDataToAll(Packet packet)
        {
            packet.InsertLength();
            for (var i = 1; i <= Server.MaxPlayers; i++) Server.Sockets[i].TcpSendData(packet);
        }

        private static void SendTcpDataToAll(int exceptClient, Packet packet)
        {
            packet.InsertLength();
            for (var i = 1; i <= Server.MaxPlayers; i++) 
                if (i != exceptClient) 
                    Server.Sockets[i].TcpSendData(packet);
        }

        private static void SendUdpData(int toClient, Packet packet)
        {
            packet.InsertLength();
            Server.Sockets[toClient].UdpSendData(packet);
        }
        
        private static void SendUdpDataToAll(Packet packet)
        {
            packet.InsertLength();
            for (var i = 1; i <= Server.MaxPlayers; i++) Server.Sockets[i].UdpSendData(packet);
        }

        private static void SendUdpDataToAll(int exceptClient, Packet packet)
        {
            packet.InsertLength();
            for (var i = 1; i <= Server.MaxPlayers; i++) 
                if (i != exceptClient) 
                    Server.Sockets[i].UdpSendData(packet);
        }
    }
}