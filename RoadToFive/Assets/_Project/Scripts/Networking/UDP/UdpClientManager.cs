using System.Net;
using _Project.Scripts.Networking.ByteArray;
using _Project.Scripts.Threading;

namespace _Project.Scripts.Networking
{
    public class UdpClientManager : IClientManager
    {
        private const string RemoteIp = "127.0.0.1";
        private const int RemotePort = 26950;

        private readonly UDP.Client _client;

        public delegate void MessageReceiveCallback(ByteArrayReader message);
        
        public UdpClientManager(MessageReceiveCallback messageReceivedCallback)
        {
            _client = new UDP.Client(RemoteIp, RemotePort, new IPEndPoint(IPAddress.Any, 0).Port);
            _client.ReceivedDatagram += (sender, receiveDatagram) =>
                MainThreadScheduler.EnqueueOnMainThread(() => messageReceivedCallback(receiveDatagram));

            _client.Listen();
            _client.SendDatagram(MessageTemplates.WriteWelcome(0, ""));
        }

        public void SetClientId(int id) => _client.Id = id;

        public void SendMessage(byte[] message) => _client.SendDatagram(message);
    }
}