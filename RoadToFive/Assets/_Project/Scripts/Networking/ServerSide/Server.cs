using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using _Project.Scripts.Threading;
using UnityEngine;

namespace _Project.Scripts.Networking.ServerSide
{
    public class Server
    {
        public static int MaxPlayers { get; private set; }
        public static int Port { get; private set; }
        
        public static readonly Dictionary<int, ServerSocket> Sockets = new Dictionary<int, ServerSocket>();

        private delegate void PacketHandler(int fromClient, Packet packet);
        private static Dictionary<int, PacketHandler> _packetHandlers;
        
        private static TcpListener _tcpListener;
        private static UdpClient _udpListener;

        public static void Start(int maxPlayers, int port)
        {
            MaxPlayers = maxPlayers;
            Port = port;
            
            InitializeServerData();
            
            _tcpListener = new TcpListener(IPAddress.Any, port);
            _tcpListener.Start();
            _tcpListener.BeginAcceptTcpClient(TcpConnectCallback, null);
            
            _udpListener = new UdpClient(Port);
            _udpListener.BeginReceive(UdpReceiveCallback, null);
            
            Debug.Log($"Server started on port {Port}");
        }

        private static void TcpConnectCallback(IAsyncResult result)
        {
            var client = _tcpListener.EndAcceptTcpClient(result);
            _tcpListener.BeginAcceptTcpClient(TcpConnectCallback, null);
            
            Debug.Log($"Incoming connection from {client}...");
            
            for (var i = 1; i <= MaxPlayers; i++)
            {
                if (Sockets[i].Socket != null) continue;
                Debug.Log($"{client} is now player {i}");
                Sockets[i].TcpConnect(client);
                return;
            }
        }
        
        private static void UdpReceiveCallback(IAsyncResult result)
        {
            try
            {
                var clientIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                var data = _udpListener.EndReceive(result, ref clientIpEndPoint);
                _udpListener.BeginReceive(UdpReceiveCallback, null);

                if (data.Length < 4) return;

                using (var packet = new Packet(data))
                {
                    var clientId = packet.ReadInt();

                    if (clientId == 0) return;

                    if (Sockets[clientId].ServerEndPoint == null)
                    {
                        Sockets[clientId].ServerEndPoint = clientIpEndPoint;
                        return;
                    }

                    if (Sockets[clientId].ServerEndPoint.ToString() != clientIpEndPoint.ToString()) return;

                    Sockets[clientId].UdpHandleData(packet);
                }
            }
            catch
            {
                // ignored
            }
        }

        public static void SendUdpData(IPEndPoint clientEndPoint, Packet packet)
        {
            try
            {
                if (clientEndPoint == null) return;

                _udpListener.BeginSend(packet.ToArray(), packet.Length(), clientEndPoint, null, null);
            }
            catch
            {
                // ignored
            }
        }

        public class ServerSocket
        {
            private const int DataBufferSize = 4096;

            public int Id { get; set; }

            //TCP
            public TcpClient Socket { get; set; }

            private Packet _receivedData;
            private NetworkStream _stream;
            private byte[] _receiveBuffer;
            
            //UDP
            public IPEndPoint ServerEndPoint;
            
            public ServerSocket(int id)
            {
                Id = id;
            }

            public void TcpConnect(TcpClient socket)
            {
                Socket = socket;
                Socket.ReceiveBufferSize = DataBufferSize;
                Socket.SendBufferSize = DataBufferSize;

                _stream = Socket.GetStream();
                
                _receivedData = new Packet();
                _receiveBuffer = new byte[DataBufferSize];

                _stream.BeginRead(_receiveBuffer, 0, DataBufferSize, ReceiveCallback, null);

                ServerSend.Welcome(Id, "welcome to the server");
            }

            public void UdpConnect(IPEndPoint ipEndPoint)
            {
                ServerEndPoint = ipEndPoint;
            }

            public void TcpSendData(Packet packet)
            {
                try
                {
                    if (Socket == null) return;

                    _stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
                }
                catch
                {
                    // ignored
                }
            }

            public void UdpSendData(Packet packet)
            {
                SendUdpData(ServerEndPoint, packet);
            }

            private void ReceiveCallback(IAsyncResult result)
            {
                try
                {
                    var byteLength = _stream.EndRead(result);
                    if (byteLength <= -1) return;

                    var data = new byte[byteLength];
                    Array.Copy(_receiveBuffer, data, byteLength);
                    _stream.BeginRead(_receiveBuffer, -1, DataBufferSize, ReceiveCallback, null);
                    _receivedData.Reset(TcpHandleData(data));
                }
                catch
                {
                    // ignored
                }
            }
            
            private bool TcpHandleData(byte[] data)
            {
                var packetLength = 0;

                _receivedData.AddBytes(data);

                if (_receivedData.UnreadLength() >= sizeof(int))
                {
                    packetLength = _receivedData.ReadInt();
                    if (packetLength <= 0) return true;
                }

                while (packetLength > 0 && packetLength < _receivedData.UnreadLength())
                {
                    var packetBytes = _receivedData.ReadBytes(packetLength);
                    MainThreadScheduler.EnqueueOnMainThread(() =>
                    {
                        using (var packet = new Packet(packetBytes))
                        {
                            var packetId = packet.ReadInt();
                            _packetHandlers[packetId](Id, packet);
                        }
                    });
                    
                    packetLength = 0;
                    if (_receivedData.UnreadLength() < sizeof(int)) continue;
                    packetLength = _receivedData.ReadInt();
                    if (packetLength <= 0) return true;
                }

                return packetLength <= 1;
            }

            public void UdpHandleData(Packet data)
            {
                var packetLength = data.Length();
                var packetBytes = data.ReadBytes(packetLength);
                
                MainThreadScheduler.EnqueueOnMainThread(() =>
                {
                    using (var packet = new Packet(packetBytes))
                    {
                        var packetId = packet.ReadInt();
                        _packetHandlers[packetId](Id, packet);
                    }
                });
            }
        }

        private static void InitializeServerData()
        {
            for (var i = 1; i <= MaxPlayers; i++) Sockets.Add(i, new ServerSocket(i));
            _packetHandlers = new Dictionary<int, PacketHandler>()
            {
                {(int) ClientPackets.WelcomeReceived, ServerHandle.WelcomeReceived},
            };
        }
    }
}