using System.Net.Sockets;

namespace _Project.Scripts.Networking.TCP
{
    public class TransmissionControlProtocolServer : TransmissionControlProtocolSocket
    {
        private readonly Server _server;

        public TransmissionControlProtocolServer(Server server)
        {
            _server = server;
        }

        public override void Connect(TcpClient socket)
        {
            Socket = socket;
            Socket.ReceiveBufferSize = DataBufferSize;
            Socket.SendBufferSize = DataBufferSize;
            
            ReceivedBuffer = new byte[DataBufferSize];
            
            NetworkStream = Socket.GetStream();
            NetworkStream.BeginRead(ReceivedBuffer, 0, DataBufferSize, ReceiveCallback, null);
        }

        protected override void HandleReadPacket(byte[] packet) => _server.ReadPacket(packet);
    }
}