using _Project.Scripts.Networking.ByteArray;
using _Project.Scripts.Threading;

namespace _Project.Scripts.Networking.TCP
{
    public class TcpServerManager : IServerManager
    {
        private const int Port = 26950;

        private readonly Server _server;
        
        public delegate void MessageReceiveCallback(ByteArrayReader message);

        /// <summary>
        /// Initializes a new tcp server that will listen on a specific port. It also initializes
        /// the receive packet event and makes sure it will run on the main thread.
        /// Then the server starts listening. On a connection it will send a welcome message
        /// to the client. 
        /// </summary>
        /// <param name="messageReceivedCallback"></param>
        public TcpServerManager(MessageReceiveCallback messageReceivedCallback)
        {
            _server = new Server(20, Port);
            _server.SetReceivedPacketHandler((sender, receivePacket) =>
                MainThreadScheduler.EnqueueOnMainThread(() => messageReceivedCallback(receivePacket)));
            _server.Listen();
        }

        public void SendMessage(int clientId, byte[] message)
        {
            _server.SendPacket(clientId, message);
        }
    }
}