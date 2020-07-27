namespace _Project.Scripts.Networking.ServerSide
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
                packet.Write(message).Write(toClient);
                SendTcpData(toClient, packet);
            }
        }

        public static void SpawnPlayer(int toClient, PlayerManager player)
        {
            using (var packet = new Packet((int)ServerPackets.SpawnPlayer))
            {
                SendTcpData(toClient, packet.Write(player.Id).Write(player.Username).Write(player.Position).Write(player.Rotation));
            }
        }

        public static void PlayerPosition(PlayerManager player)
        {
            using (var packet = new Packet((int) ServerPackets.PlayerPosition))
            {
                packet.Write(player.Id).Write(player.Position);
                
                SendUdpDataToAll(packet);
            }
        }
        
        public static void PlayerRotation(PlayerManager player)
        {
            using (var packet = new Packet((int) ServerPackets.PlayerRotation))
            {
                packet.Write(player.Id).Write(player.Rotation);
                
                SendUdpDataToAll(player.Id, packet);
            }
        }
    }
}