using _Project.Scripts.Networking.ByteArray;
using _Project.Scripts.Threading;

namespace _Project.Scripts.Networking
{
    public class UdpServerManager : IServerManager
    {
        private const int Port = 26950;

        private readonly UDP.Server _server;
        public delegate void MessageReceiveCallback(ByteArrayReader message);

        public UdpServerManager(MessageReceiveCallback messageReceivedCallback)
        {
            _server = new UDP.Server(Port);
            _server.ReceivedDatagram += (sender, receiveDatagram) =>
                MainThreadScheduler.EnqueueOnMainThread(() => messageReceivedCallback(receiveDatagram));

            _server.Listen();
        }

        public void SendMessage(int clientId, byte[] message) => _server.SendDatagram(message, _server.GetHost(clientId));
    }
}