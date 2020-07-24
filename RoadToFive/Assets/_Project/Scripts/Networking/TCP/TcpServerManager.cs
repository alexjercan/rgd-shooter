using _Project.Scripts.Networking.ByteArray;
using _Project.Scripts.Threading;

namespace _Project.Scripts.Networking
{
    public class TcpServerManager : IServerManager
    {
        private const int Port = 26950;

        private readonly TCP.Server _server;
        
        public delegate void MessageReceiveCallback(ByteArrayReader message);

        public TcpServerManager(MessageReceiveCallback messageReceivedCallback)
        {
            _server = new TCP.Server(20, Port);
            _server.SetReceivedDatagramHandler((sender, receivePacket) =>
                MainThreadScheduler.EnqueueOnMainThread(() => messageReceivedCallback(receivePacket)));
            _server.Listen();
        }

        public void SendMessage(int clientId, byte[] message)
        {
            _server.SendPacket(clientId, message);
        }
    }
}