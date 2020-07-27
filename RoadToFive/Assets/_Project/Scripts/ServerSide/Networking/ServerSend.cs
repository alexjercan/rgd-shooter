using _Project.Scripts.DataStructure;

namespace _Project.Scripts.ServerSide.Networking
{
    public static class ServerSend
    {
        private static void SendTcpData(int toClient, Packet packet)
        {
            packet.InsertLength();
            Server.ClientConnections[toClient].Tcp.SendData(packet);
        }

        private static void SendTcpDataToAll(Packet packet)
        {
            packet.InsertLength();
            for (var i = 1; i <= Server.MaxPlayers; i++) Server.ClientConnections[i].Tcp.SendData(packet);
        }

        private static void SendTcpDataToAll(int exceptClient, Packet packet)
        {
            packet.InsertLength();
            for (var i = 1; i <= Server.MaxPlayers; i++) 
                if (i != exceptClient) 
                    Server.ClientConnections[i].Tcp.SendData(packet);
        }

        private static void SendUdpData(int toClient, Packet packet)
        {
            packet.InsertLength();
            Server.ClientConnections[toClient].Udp.SendData(packet);
        }
        
        private static void SendUdpDataToAll(Packet packet)
        {
            packet.InsertLength();
            for (var i = 1; i <= Server.MaxPlayers; i++) Server.ClientConnections[i].Udp.SendData(packet);
        }

        private static void SendUdpDataToAll(int exceptClient, Packet packet)
        {
            packet.InsertLength();
            for (var i = 1; i <= Server.MaxPlayers; i++) 
                if (i != exceptClient) 
                    Server.ClientConnections[i].Udp.SendData(packet);
        }
        
        public static void Welcome(int toClient, string message)
        {
            using (var packet = new Packet((int) ServerPackets.Welcome))
            {
                SendTcpData(toClient, packet.Write(message).Write(toClient));
            }
        }

        public static void SpawnPlayer(int toClient, ServerPlayerManager player)
        {
            using (var packet = new Packet((int)ServerPackets.SpawnPlayer))
            {
                SendTcpData(toClient, packet.Write(player.Id).Write(player.Username).Write(player.PlayerTransform.position).Write(player.PlayerTransform.rotation));
            }
        }

        public static void PlayerPosition(ServerPlayerManager player)
        {
            using (var packet = new Packet((int) ServerPackets.PlayerPosition))
            {
                SendUdpDataToAll(packet.Write(player.Id).Write(player.PlayerTransform.position));
            }
        }
        
        public static void PlayerRotation(ServerPlayerManager player)
        {
            using (var packet = new Packet((int) ServerPackets.PlayerRotation))
            {
                SendUdpDataToAll(player.Id, packet.Write(player.Id).Write(player.PlayerTransform.rotation));
            }
        }

        public static void PlayerDisconnected(int playerId)
        {
            using (var packet = new Packet((int) ServerPackets.PlayerDisconnected))
            {
                SendTcpDataToAll(packet.Write(playerId));
            }
        }
    }
}