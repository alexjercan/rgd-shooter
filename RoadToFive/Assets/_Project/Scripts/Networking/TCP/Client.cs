using _Project.Scripts.Networking.ByteArray;

namespace _Project.Scripts.Networking.TCP
{
    public class Client
    {
        private readonly ClientSocket _socket;

        /// <summary>
        /// Creates a new socket to communicate with the remote host.
        /// </summary>
        /// <param name="serverIp"></param>
        /// <param name="port"></param>
        public Client(string serverIp, int port) => _socket = new ClientSocket(serverIp, port);

        /// <summary>
        /// Connects to the remote host.
        /// </summary>
        public void Connect() => _socket.Connect();

        public void Disconnect() => _socket.Disconnect();

        public void SendPacket(byte[] data) => _socket.SendPacket(data);

        public void SetReceivedDatagramHandler(ReceivedHandler handler) => _socket.ReceivedPacket += (sender, reader) => handler(sender, reader);
        
        public delegate void ReceivedHandler(object sender, ByteArrayReader reader);
    }
}