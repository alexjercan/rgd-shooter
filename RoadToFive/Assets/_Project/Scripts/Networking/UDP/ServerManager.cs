using _Project.Scripts.Networking.ByteArray;
using _Project.Scripts.Networking.Threading;

namespace _Project.Scripts.Networking.UDP
{
    public class ServerManager
    {
        /// <summary>
        /// The port on which the server runs
        /// </summary>
        private const int Port = 26950;

        private readonly Server _server;
        public delegate void MessageReceiveCallback(ByteArrayReader message);

        /// <summary>
        /// Class constructor. It creates a new server instance and binds its socket to the local port
        /// on which the server will listen for messages. It initializes the receive event with a callback
        /// and then starts listening for messages. If a incoming message has the id 0 it means that it's
        /// a new client and the server adds it to the known hosts, and sends its new udp id.
        /// </summary>
        /// <param name="messageReceivedCallback"></param>
        public ServerManager(MessageReceiveCallback messageReceivedCallback)
        {
            _server = new Server(Port);
            _server.ReceivedDatagram += (sender, receiveDatagram) =>
                MainThreadScheduler.EnqueueOnMainThread(() => messageReceivedCallback(receiveDatagram));

            _server.Listen();
        }
        
        public void SendMessage(int clientId, byte[] message) => _server.SendDatagram(message, clientId);

        public void BroadcastMessage(byte[] message) => _server.BroadcastDatagram(message);
        
        public void BroadcastMessageExcept(int clientId, byte[] message) =>
            _server.BroadcastDatagramExcept(clientId, message);

        public void Disconnect() => _server.Disconnect();
    }
}