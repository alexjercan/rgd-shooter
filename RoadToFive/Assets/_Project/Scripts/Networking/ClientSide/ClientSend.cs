namespace _Project.Scripts.Networking.ClientSide
{
    public class ClientSend
    {
        public static void WelcomeReceived()
        {
            using (var packet = new Packet((int) ClientPackets.WelcomeReceived))
            {
                packet.Write(Client.MyId).Write("GUEST " + Client.MyId);
                
                SendTcpData(packet);
            }
        }

        private static void SendTcpData(Packet packet)
        {
            packet.InsertLength();
            Client.Socket.TcpSendData(packet);
        }

        private static void SendUdpData(Packet packet)
        {
            packet.InsertLength();
            Client.Socket.UdpSendData(packet);
        }
    }
}