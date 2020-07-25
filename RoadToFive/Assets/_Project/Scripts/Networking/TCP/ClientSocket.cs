using System;
using System.Net.Sockets;
using _Project.Scripts.Networking.ByteArray;

namespace _Project.Scripts.Networking.TCP
{
    public class ClientSocket
    {
        /// <summary>
        /// This event is being invoked when a message is received by the socket.
        /// </summary>
        public event EventHandler<ByteArrayReader> ReceivedPacket;

        private TcpClient _socket;

        private const int DataBufferSize = 4096;

        private NetworkStream _networkStream;
        private byte[] _receivedBuffer;
        private ByteArrayReader _receivedByteArrayReader = new ByteArrayReader();

        private readonly string _ipServer;
        private readonly int _port;

        /// <summary>
        /// Initialize a socket with the ip and port of the remote host.
        /// </summary>
        /// <param name="ipServer"></param>
        /// <param name="port"></param>
        public ClientSocket(string ipServer, int port)
        {
            _ipServer = ipServer;
            _port = port;
        }

        /// <summary>
        /// Begins connecting to the remote host
        /// </summary>
        public void Connect()
        {
            _socket = new TcpClient {ReceiveBufferSize = DataBufferSize, SendBufferSize = DataBufferSize};

            _receivedBuffer = new byte[DataBufferSize];
            _socket.BeginConnect(_ipServer, _port, ConnectCallback, _socket);
        }
        
        /// <summary>
        /// Sends a packet to the remote host.
        /// </summary>
        /// <param name="packet"></param>
        public void SendPacket(byte[] packet)
        {
            if (_socket == null) return;
            _networkStream.BeginWrite(packet, 0, packet.Length, null, null);
        }

        public void Disconnect()
        {
            if (_socket == null || !_socket.Connected) return;
            
            Console.WriteLine("Disconnecting from server!");
            _socket.Close();
            _networkStream = null;
            _receivedBuffer = null;
            _receivedByteArrayReader = null;
            _socket = null;
        }

        /// <summary>
        /// When the connection is established the client starts listening on the socket's
        /// network stream for packets
        /// </summary>
        /// <param name="asyncResult"></param>
        private void ConnectCallback(IAsyncResult asyncResult)
        {
            _socket.EndConnect(asyncResult);

            if (!_socket.Connected) return;

            _networkStream = _socket.GetStream();
            _networkStream.BeginRead(_receivedBuffer, 0, DataBufferSize, ReceiveCallback, null);
        }

        /// <summary>
        /// When a packet is received check if it is fragmented and wait for the rest of the packet to arrive
        /// otherwise read the entire packet from the buffer and process it.
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
        
        /// <summary>
        /// Used to check if a packet is fragmented. If it is not the process it.
        /// </summary>
        /// <param name="receivedData"></param>
        /// <returns></returns>
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