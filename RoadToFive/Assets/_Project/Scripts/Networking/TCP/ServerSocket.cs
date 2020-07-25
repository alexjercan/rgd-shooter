using System;
using System.Net.Sockets;
using _Project.Scripts.Logging;
using _Project.Scripts.Networking.ByteArray;

namespace _Project.Scripts.Networking.TCP
{
    public class ServerSocket
    {
        /// <summary>
        /// This event is being invoked when a message is received by the socket.
        /// </summary>
        public event EventHandler<ByteArrayReader> ReceivedPacket;

        public TcpClient Socket { get; private set; }

        private const int DataBufferSize = 4096;

        private NetworkStream _networkStream;
        private byte[] _receivedBuffer;
        private ByteArrayReader _receivedByteArrayReader;

        /// <summary>
        /// Creates a socket with the specified information representing a client. This socket
        /// will be used so send information and receiving information from the client.
        /// Afterwards the server will start listening on the socket's network stream.
        /// </summary>
        /// <param name="socket"></param>
        public void Connect(TcpClient socket)
        {
            Socket = socket;
            Socket.ReceiveBufferSize = DataBufferSize;
            Socket.SendBufferSize = DataBufferSize;
            
            _receivedBuffer = new byte[DataBufferSize];
            _receivedByteArrayReader = new ByteArrayReader();
            
            _networkStream = Socket.GetStream();
            _networkStream.BeginRead(_receivedBuffer, 0, DataBufferSize, ReceiveCallback, null);
        }

        /// <summary>
        /// Writes data in the socket's network stream, this resulting in sending a packet to the client
        /// represented by the socket.
        /// </summary>
        /// <param name="packet"></param>
        public void SendPacket(byte[] packet)
        {
            if (Socket == null) return;
            _networkStream.BeginWrite(packet, 0, packet.Length, null, null);
        }

        /// <summary>
        /// Handle the case when a client disconnects from the server. Close the socket and free the buffers.
        /// </summary>
        public void Disconnect()
        {
            Console.WriteLine($"{Socket.Client.RemoteEndPoint} has disconnected");
            Socket.Close();
            _networkStream = null;
            _receivedBuffer = null;
            _receivedByteArrayReader = null;
            Socket = null;
        }
        
        /// <summary>
        /// When receiving data, the server must check, in the same way the client does, if the packet is
        /// or not fragmented. If it is not the the server can proceed to processing it.
        /// </summary>
        /// <param name="asyncResult"></param>
        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            var byteLength = _networkStream.EndRead(asyncResult);
            if (byteLength <= 0)
            {
                Disconnect();
                return;
            }

            var data = new byte[byteLength];
            Array.Copy(_receivedBuffer, data, byteLength);

            if (ReceivedDataHandler(data)) _receivedByteArrayReader = new ByteArrayReader();
            _networkStream.BeginRead(_receivedBuffer, 0, DataBufferSize, ReceiveCallback, null);
        }
        
        private bool ReceivedDataHandler(byte[] receivedData)
        {
            var packetLength = 0;
            _receivedByteArrayReader.AddBytes(receivedData);
            
            if (_receivedByteArrayReader.UnreadBytes >= sizeof(int))
            {
                packetLength = _receivedByteArrayReader.ReadInt();
                if (packetLength <= 0) return true;
            }

            while (packetLength > 0 && packetLength <= _receivedByteArrayReader.UnreadBytes)
            {
                var bytes = _receivedByteArrayReader.ReadBytes(packetLength);
                ReceivedPacket?.Invoke(this, new ByteArrayReader(bytes));
                
                packetLength = 0;
                if (_receivedByteArrayReader.UnreadBytes < sizeof(int)) continue;
                packetLength = _receivedByteArrayReader.ReadInt();
                if (packetLength <= 0) return true;
            }

            return packetLength <= 1;
        }
        
    }
}