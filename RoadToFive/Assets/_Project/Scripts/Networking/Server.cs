using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using _Project.Scripts.ByteArray;
using _Project.Scripts.Threading;

namespace _Project.Scripts.Networking
{
    public class Server
    {
        public delegate void MessageReceiveCallback(int clientId, ByteArrayReader message);
        private readonly MessageReceiveCallback _messageReceivedCallback;

        private readonly int _maxPlayerCount;
        private readonly int _port;

        private readonly Dictionary<int, ServerSocket> _sockets = new Dictionary<int, ServerSocket>();

        private readonly TcpListener _tcpListener;
        private readonly UdpClient _udpListener;
        
        private IPEndPoint _clientEndPoint;

        public Server(int maxPlayerCount, int port, MessageReceiveCallback messageReceivedCallback)
        {
            _messageReceivedCallback = messageReceivedCallback;
            _maxPlayerCount = maxPlayerCount;
            _port = port;

            for (var i = 1; i <= _maxPlayerCount; i++) _sockets.Add(i, new ServerSocket(_messageReceivedCallback));

            _tcpListener = new TcpListener(IPAddress.Any, _port);
            _udpListener = new UdpClient(_port);
            
            _clientEndPoint = new IPEndPoint(IPAddress.Any, port);
        }
        
        public void Listen()
        {
            _tcpListener.Start();
            
            _udpListener.BeginReceive(UdpReceiveCallback, null);
            _tcpListener.BeginAcceptTcpClient(TcpConnectCallback, null);

            Console.WriteLine($"Server started on port {_port}");
        }
        
        private void TcpConnectCallback(IAsyncResult asyncResult)
        {
            var client = _tcpListener.EndAcceptTcpClient(asyncResult);
            _tcpListener.BeginAcceptTcpClient(TcpConnectCallback, null);
            Console.WriteLine($"Incoming connection from {client.Client.RemoteEndPoint}...");

            for (var i = 1; i <= _maxPlayerCount; i++)
            {
                if (_sockets[i].Socket != null) continue;

                _sockets[i].Connect(client);
                SendTcpMessage(i, MessageTemplates.WriteWelcome());
                return;
            }
        }
        
        private void UdpReceiveCallback(IAsyncResult asyncResult)
        {
            try
            {
                var receivedBytes = _udpListener.EndReceive(asyncResult, ref _clientEndPoint);
                _udpListener.BeginReceive(UdpReceiveCallback, null);

                if (receivedBytes.Length < 4)
                    return;

                var receiveDatagram = new ByteArrayReader(receivedBytes);
                var clientId = receiveDatagram.ReadInt();

                if (clientId == 0) return;

                if (_sockets[clientId].ClientEndPoint == null)
                {
                    _sockets[clientId].ClientEndPoint = _clientEndPoint;
                    return;
                }

                if (_sockets[clientId].ClientEndPoint.ToString() != _clientEndPoint.ToString()) return;

                var datagramLength = receiveDatagram.ReadInt();
                if (datagramLength != receiveDatagram.UnreadBytes) return;

                MainThreadScheduler.EnqueueOnMainThread(() => _messageReceivedCallback(clientId, receiveDatagram));
            }
            catch (Exception e)
            {
                // ignored
            }
        }
        
        public void SendTcpMessage(int clientId, byte[] data)
        {
            var byteArrayBuilder = new ByteArrayBuilder().Write(clientId).Write(data).ToByteArray();
            _sockets[clientId].SendPacket(byteArrayBuilder);
        }

        public void SendUdpMessage(int clientId, byte[] datagram)
        {
            if (_sockets[clientId].ClientEndPoint == null) return;
            
            var byteArrayBuilder = new ByteArrayBuilder().Write(clientId).Write(datagram).ToByteArray();
            _udpListener.BeginSend(byteArrayBuilder, byteArrayBuilder.Length, _sockets[clientId].ClientEndPoint, null, null);
        }

        public void BroadcastTcp(byte[] data)
        {
            foreach (var clientId in _sockets.Keys)
                SendTcpMessage(clientId, data);
        }
        
        public void BroadcastUdp(byte[] datagram)
        {
            foreach (var hostId in _sockets.Keys) 
                SendUdpMessage(hostId, datagram);
        }
        
        public void RemoveClient(int clientId) => _sockets[clientId].Disconnect();

        public void Stop()
        {
            _tcpListener.Stop();
            _udpListener.Close();
            
            foreach (var socket in _sockets.Values) socket.Disconnect();
        }
        
        private class ServerSocket
        {
            public TcpClient Socket { get; private set; }
            public IPEndPoint ClientEndPoint { get; set; }

            private const int DataBufferSize = 4096;

            private NetworkStream _networkStream;
            private byte[] _receivedBuffer;
            private ByteArrayReader _receivedByteArrayReader;

            private readonly MessageReceiveCallback _messageReceivedCallback;

            public ServerSocket(MessageReceiveCallback messageReceivedCallback)
            {
                _messageReceivedCallback = messageReceivedCallback;
                ClientEndPoint = null;
            }

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
            
            public void SendPacket(byte[] packet)
            {
                if (Socket == null) return;
                _networkStream.BeginWrite(packet, 0, packet.Length, null, null);
            }
            
            public void Disconnect()
            {
                if (Socket == null) return;
                Socket.Close();
                _networkStream = null;
                _receivedBuffer = null;
                _receivedByteArrayReader = null;
                Socket = null;
                ClientEndPoint = null;
            }

            private void ReceiveCallback(IAsyncResult asyncResult)
            {
                var byteLength = _networkStream.EndRead(asyncResult);
                if (byteLength <= 0) return;

                var data = new byte[byteLength];
                Array.Copy(_receivedBuffer, data, byteLength);

                if (ReceivedDataHandler(data)) _receivedByteArrayReader = new ByteArrayReader();
                _networkStream.BeginRead(_receivedBuffer, 0, DataBufferSize, ReceiveCallback, null);
            }
            
            private bool ReceivedDataHandler(byte[] receivedData)
            {
                var packetLength = 0;
                var clientId = 0;
                _receivedByteArrayReader.AddBytes(receivedData);
                
                if (_receivedByteArrayReader.UnreadBytes >= 2 * sizeof(int))
                {
                    clientId = _receivedByteArrayReader.ReadInt();
                    packetLength = _receivedByteArrayReader.ReadInt();
                    if (packetLength <= 0) return true;
                }

                while (packetLength > 0 && packetLength <= _receivedByteArrayReader.UnreadBytes)
                {
                    var bytes = _receivedByteArrayReader.ReadBytes(packetLength);
                    var id = clientId;
                    MainThreadScheduler.EnqueueOnMainThread(() => _messageReceivedCallback(id, new ByteArrayReader(bytes)));
                    
                    packetLength = 0;
                    if (_receivedByteArrayReader.UnreadBytes < 2 * sizeof(int)) continue;
                    clientId = _receivedByteArrayReader.ReadInt();
                    packetLength = _receivedByteArrayReader.ReadInt();
                    if (packetLength <= 0) return true;
                }

                return packetLength <= 1;
            }
        }
    }
}