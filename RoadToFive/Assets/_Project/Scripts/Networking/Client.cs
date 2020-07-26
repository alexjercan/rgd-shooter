using System;
using System.Net;
using System.Net.Sockets;
using _Project.Scripts.ByteArray;
using _Project.Scripts.Threading;
using UnityEngine;

namespace _Project.Scripts.Networking
{
    public class Client
    {
        public int Id { get; set; }
        public delegate void MessageReceiveCallback(ByteArrayReader message);
        private readonly MessageReceiveCallback _messageReceivedCallback;

        private IPEndPoint _remoteHostEndPoint;
        private readonly ClientSocket _tcpSocket;
        private UdpClient _udpSocket;

        public Client(string remoteIp, int remotePort, MessageReceiveCallback messageReceivedCallback)
        {
            _remoteHostEndPoint = new IPEndPoint(IPAddress.Parse(remoteIp), remotePort);
            _messageReceivedCallback = messageReceivedCallback;
            
            _udpSocket = new UdpClient();
            _tcpSocket = new ClientSocket(remoteIp, remotePort, _messageReceivedCallback);
            _udpSocket.Connect(_remoteHostEndPoint);
        }

        public void Listen()
        {
            _tcpSocket.Connect();
            _udpSocket.BeginReceive(UdpReceiveCallback, null);
        }

        private void UdpReceiveCallback(IAsyncResult asyncResult)
        {
            try
            {
                var receivedBytes = _udpSocket.EndReceive(asyncResult, ref _remoteHostEndPoint);
                _udpSocket.BeginReceive(UdpReceiveCallback, null);

                if (receivedBytes.Length < 4) return;

                var receiveDatagram = new ByteArrayReader(receivedBytes);
                var datagramLength = receiveDatagram.ReadInt();
                if (datagramLength != receiveDatagram.UnreadBytes) return;
                MainThreadScheduler.EnqueueOnMainThread(() => _messageReceivedCallback(receiveDatagram));
            }
            catch (Exception e)
            {
                // ignored
            }
        }
        
        public void SendUdpMessage(int senderId, byte[] datagram)
        {
            //TODO FIX BUG : SERVER DOES NOT RECEIVE MESSAGES ON PUBLIC IP
            var byteArrayBuilder = new ByteArrayBuilder().Write(senderId).Write(datagram).ToByteArray();
            _udpSocket?.BeginSend(byteArrayBuilder, byteArrayBuilder.Length, null, null);
        }

        public void SendTcpMessage(byte[] packet) => _tcpSocket.SendPacket(packet);

        public void Disconnect()
        {
            _tcpSocket.Disconnect();
            
            if (_udpSocket == null) return;

            _udpSocket.Close();
            _udpSocket = null;
            _remoteHostEndPoint = null;
        }

        private class ClientSocket
        {
            private readonly MessageReceiveCallback _messageReceivedCallback;

            private TcpClient _socket;

            private const int DataBufferSize = 4096;

            private NetworkStream _networkStream;
            private byte[] _receivedBuffer;
            private ByteArrayReader _receivedByteArrayReader = new ByteArrayReader();

            private readonly string _ipServer;
            private readonly int _port;

            public ClientSocket(string ipServer, int port, MessageReceiveCallback messageReceivedCallback)
            {
                _messageReceivedCallback = messageReceivedCallback;
                _ipServer = ipServer;
                _port = port;
            }

            public void Connect()
            {
                _socket = new TcpClient {ReceiveBufferSize = DataBufferSize, SendBufferSize = DataBufferSize};

                _receivedBuffer = new byte[DataBufferSize];
                _socket.BeginConnect(_ipServer, _port, ConnectCallback, _socket);
            }
            
            public void SendPacket(byte[] packet)
            {
                if (_socket == null) return;
                _networkStream.BeginWrite(packet, 0, packet.Length, null, null);
            }

            private void ConnectCallback(IAsyncResult asyncResult)
            {
                _socket.EndConnect(asyncResult);

                if (!_socket.Connected) return;

                _networkStream = _socket.GetStream();
                _networkStream.BeginRead(_receivedBuffer, 0, DataBufferSize, ReceiveCallback, null);
            }

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
                    MainThreadScheduler.EnqueueOnMainThread(() => _messageReceivedCallback(new ByteArrayReader(bytes)));

                    packetLength = 0;
                    if (_receivedByteArrayReader.UnreadBytes < sizeof(int)) continue;
                    packetLength = _receivedByteArrayReader.ReadInt();
                    if (packetLength <= 0) return true;
                }

                return packetLength <= 1;
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
        }
    }
}