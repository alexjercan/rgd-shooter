using System.Net.Sockets;

namespace _Project.Scripts.Networking.TCP
{
    public class TcpConnectionServer : TcpConnection
    {
        private readonly ServerTcp _serverTcp;

        public TcpConnectionServer(ServerTcp serverTcp)
        {
            _serverTcp = serverTcp;
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

        protected override void HandleReadPacket(byte[] packet) => _serverTcp.ReadPacket(packet);
    }
}