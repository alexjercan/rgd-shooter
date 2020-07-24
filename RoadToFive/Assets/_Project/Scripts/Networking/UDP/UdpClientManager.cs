using System.Net;
using _Project.Scripts.Networking.ByteArray;
using _Project.Scripts.Networking.Threading;

namespace _Project.Scripts.Networking.UDP
{
    public class UdpClientManager : IClientManager
    {
        /// <summary>
        /// Server ip and port
        /// </summary>
        private const string RemoteIp = "127.0.0.1";
        private const int RemotePort = 26950;

        private readonly Client _client;

        public delegate void MessageReceiveCallback(ByteArrayReader message);
        
        /// <summary>
        /// Class constructor. It creates a new client instance and binds the udp socket
        /// to the remote host. It also makes sure that the receive callback will be called
        /// each time a new datagram is received. Starts listening asynchronously.
        /// </summary>
        /// <param name="messageReceivedCallback"></param>
        public UdpClientManager(MessageReceiveCallback messageReceivedCallback)
        {
            _client = new Client(RemoteIp, RemotePort, new IPEndPoint(IPAddress.Any, 0).Port);
            _client.ReceivedDatagram += (sender, receiveDatagram) =>
                MainThreadScheduler.EnqueueOnMainThread(() => messageReceivedCallback(receiveDatagram));

            _client.Listen();
        }

        /// <summary>
        /// Before sending the message to the default remote host set by the constructor,
        /// the client id is inserted at the beginning.
        /// </summary>
        /// <param name="senderId"></param>
        /// <param name="message"></param>
        public void SendMessage(int senderId, byte[] message) => _client.SendDatagram(senderId, message);
    }
}