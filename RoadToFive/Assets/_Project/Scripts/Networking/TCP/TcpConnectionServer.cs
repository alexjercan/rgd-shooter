using System.Net.Sockets;

namespace _Project.Scripts.Networking.TCP
{
    public class TcpConnectionServer : TcpConnection
    {
        //This class represents a new tcp connection on the server side.
        //It exposes a socket that the server uses to communicate with the client.

        public override void Connect(TcpClient socket)
        {
            Socket = socket;
            Socket.ReceiveBufferSize = DataBufferSize;
            Socket.SendBufferSize = DataBufferSize;
            
            ReceivedBuffer = new byte[DataBufferSize];
            
            NetworkStream = Socket.GetStream();
            NetworkStream.BeginRead(ReceivedBuffer, 0, DataBufferSize, ReceiveCallback, null);
        }
        
        public override void SendPacket(byte[] packet)
        {
            if (Socket == null) return;

            NetworkStream.BeginWrite(packet, 0, packet.Length, null, null);
        }
    }
}