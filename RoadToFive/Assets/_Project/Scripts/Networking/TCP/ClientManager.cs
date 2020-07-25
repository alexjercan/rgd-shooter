using _Project.Scripts.Networking.ByteArray;
using _Project.Scripts.Networking.Threading;

namespace _Project.Scripts.Networking.TCP
{
    public class ClientManager
    {
        private const string RemoteIp = "127.0.0.1";
        private const int RemotePort = 26950;

        private readonly Client _client;

        public delegate void MessageReceiveCallback(ByteArrayReader message);
        
        /// <summary>
        /// It creates a new instance of a client, initializes the receive event with a callback
        /// and starts a connection to the server.
        /// </summary>
        /// <param name="messageReceivedCallback"></param>
        public ClientManager(MessageReceiveCallback messageReceivedCallback)
        {
            _client = new Client(RemoteIp, RemotePort);
            _client.SetReceivedDatagramHandler((sender, receivePacket) =>
                MainThreadScheduler.EnqueueOnMainThread(() => messageReceivedCallback(receivePacket)));
        }
        
        public void Connect() => _client.Connect();
        
        public void SendMessage(byte[] message) => _client.SendPacket(message);

        public void Disconnect() => _client.Disconnect();
    }
}