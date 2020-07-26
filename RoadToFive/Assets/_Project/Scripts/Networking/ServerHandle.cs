namespace _Project.Scripts.Networking
{
    class ServerHandle
    {
        public static void WelcomeReceived(int fromClient, Packet packet)
        {
            var clientIdCheck = packet.ReadInt();
            var username = packet.ReadString();

            if (fromClient != clientIdCheck) return;
            
            // TODO: send player into game
        }
    }
}