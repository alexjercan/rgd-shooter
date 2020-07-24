using _Project.Scripts.Networking.ByteArray;
using _Project.Scripts.Threading;

namespace _Project.Scripts.Networking
{
    public class TcpClientManager : IClientManager
    {
        private const string RemoteIp = "127.0.0.1";
        private const int RemotePort = 26950;

        private readonly TCP.Client _client;

        public delegate void MessageReceiveCallback(ByteArrayReader message);
        
        public TcpClientManager(MessageReceiveCallback messageReceivedCallback)
        {
            _client = new TCP.Client(RemoteIp, RemotePort);
            _client.SetReceivedDatagramHandler((sender, receivePacket) =>
                MainThreadScheduler.EnqueueOnMainThread(() => messageReceivedCallback(receivePacket)));
            _client.Connect();
        }

        public void SetClientId(int id) => _client.Id = id;
        public void SendMessage(byte[] message) => _client.SendPacket(message);
    }
}