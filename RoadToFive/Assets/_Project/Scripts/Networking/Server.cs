using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using _Project.Scripts.Networking.ByteArray;
using _Project.Scripts.Networking.Threading;
using UnityEngine;

namespace _Project.Scripts.Networking
{
    public class Server
    {
        public delegate void MessageReceiveCallback(ByteArrayReader message);
        private readonly MessageReceiveCallback _messageReceivedCallback;

        private readonly int _maxPlayerCount;
        private readonly int _port;

        private readonly Dictionary<int, ServerSocket> _sockets = new Dictionary<int, ServerSocket>();

        private readonly TcpListener _tcpListener;
        private readonly UdpClient _udpListener;
        
        private IPEndPoint clientEndPoint;

        public Server(int maxPlayerCount, int port, MessageReceiveCallback messageReceivedCallback)
        {
            _messageReceivedCallback = messageReceivedCallback;
            _maxPlayerCount = maxPlayerCount;
            _port = port;

            for (var i = 1; i <= _maxPlayerCount; i++) _sockets.Add(i, new ServerSocket(_messageReceivedCallback));

            _tcpListener = new TcpListener(IPAddress.Any, _port);
            _udpListener = new UdpClient(_port);
            
            clientEndPoint = new IPEndPoint(IPAddress.Any, port);
        }
        
        public void Listen()
        {
            _tcpListener.Start();
            
            _udpListener.BeginReceive(UdpReceiveCallback, null);
            _tcpListener.BeginAcceptTcpClient(TcpConnectCallback, null);

            Debug.Log($"TCP local endpoint {_tcpListener.Server.LocalEndPoint}");
            Debug.Log($"TCP remote endpoint {_tcpListener.Server.RemoteEndPoint}");
            Debug.Log($"UDP local endpoint {_udpListener.Client.LocalEndPoint}");
            Debug.Log($"UDP remote endpoint {_udpListener.Client.RemoteEndPoint}");
            
            Debug.Log($"Server started on port {_port}");
        }
        
        private void TcpConnectCallback(IAsyncResult asyncResult)
        {
            var client = _tcpListener.EndAcceptTcpClient(asyncResult);
            _tcpListener.BeginAcceptTcpClient(TcpConnectCallback, null);
            Console.Write($"Incoming connection from {client.Client.RemoteEndPoint}...");

            for (var i = 1; i <= _maxPlayerCount; i++)
            {
                if (_sockets[i].Socket != null) continue;
                
                Debug.Log($" connecting it on socket {i}");
                _sockets[i].Connect(client);
                SendTcpMessage(i, MessageTemplates.WriteWelcome(i));
                return;
            }
            
            Debug.Log(" failed to connect: Server full!");
        }
        
        private void UdpReceiveCallback(IAsyncResult asyncResult)
        {
            try
            {
                var receivedBytes = _udpListener.EndReceive(asyncResult, ref clientEndPoint);
                Debug.Log($"received message from {clientEndPoint}");
                _udpListener.BeginReceive(UdpReceiveCallback, null);

                if (receivedBytes.Length < 4)
                {
                    Debug.Log("Error reading datagram: receivedBytes is to short!");
                    return;
                }
                
                var receiveDatagram = new ByteArrayReader(receivedBytes);
                var clientId = receiveDatagram.ReadInt();

                if (clientId == 0) return;

                if (_sockets[clientId].ClientEndPoint == null)
                {
                    _sockets[clientId].ClientEndPoint =  clientEndPoint;
                    return;
                }

                if (_sockets[clientId].ClientEndPoint.ToString() != clientEndPoint.ToString())
                {
                    Debug.Log($"{clientId} assumed wrong endpoint!");
                    return;
                }

                var datagramLength = receiveDatagram.ReadInt();
                if (datagramLength != receiveDatagram.UnreadBytes)
                {
                    Debug.Log("Error with datagram length");
                    return;
                }
                
                MainThreadScheduler.EnqueueOnMainThread(() => _messageReceivedCallback(receiveDatagram));
            }
            catch (Exception e)
            {
                Debug.Log($"Error receiving message UDP: {e}");
            }
        }
        
        public void SendTcpMessage(int client, byte[] data) => _sockets[client].SendPacket(data);
        
        public void SendUdpMessage(byte[] datagram, int hostId)
        {
            if (_sockets[hostId].ClientEndPoint == null) return;
            _udpListener.BeginSend(datagram, datagram.Length, _sockets[hostId].ClientEndPoint, null, null);
        }

        public void BroadcastTcp(byte[] data)
        {
            foreach (var connection in _sockets.Values ) 
                connection.SendPacket(data);
        }
        
        public void BroadcastUdp(byte[] datagram)
        {
            foreach (var hostId in _sockets.Keys) 
                SendUdpMessage(datagram, hostId);
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
                Debug.Log($"{Socket.Client.RemoteEndPoint} has disconnected");
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
        }
    }
}