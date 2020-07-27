using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using _Project.Scripts.Threading;
using UnityEngine;

namespace _Project.Scripts.Networking.ClientSide
{
    public class Client
    {
        public static string ServerIp { get; set; }
        public static int ServerPort { get; set; }
        public static int MyId { get; set; }
        public static ClientSocket Socket { get; set; }

        private delegate void PacketHandler(Packet packet);

        private static Dictionary<int, PacketHandler> _packetHandlers;
        public static void ConnectToServer(string ip)
        {
            Debug.Log($"Connecting to server {ip}");
            ServerPort = 26950;
            ServerIp = ip;
            Socket = new ClientSocket();
            InitializeClientData();
            Socket.TcpConnect();
        }

        public class ClientSocket
        {
            private const int DataBufferSize = 4096;

            //TCP
            public TcpClient TcpSocket;

            private Packet _receivedData;
            private NetworkStream _stream;
            private byte[] _receiveBuffer;
            
            //UDP
            public UdpClient UdpSocket;
            public IPEndPoint ClientEndPoint;

            public ClientSocket()
            {
                ClientEndPoint = new IPEndPoint(IPAddress.Parse(ServerIp), ServerPort);
            }
            
            public void TcpConnect()
            {
                TcpSocket = new TcpClient
                {
                    ReceiveBufferSize = DataBufferSize,
                    SendBufferSize = DataBufferSize
                };

                _receiveBuffer = new byte[DataBufferSize];
                TcpSocket.BeginConnect(ServerIp, ServerPort, ConnectCallback, TcpSocket);
            }

            public void UdpConnect(int localPort)
            {
                UdpSocket = new UdpClient(localPort);
                
                UdpSocket.Connect(ClientEndPoint);
                UdpSocket.BeginReceive(UdpReceiveCallback, null);

                using (var packet = new Packet())
                {
                    UdpSendData(packet);
                }
            }

            private void ConnectCallback(IAsyncResult result)
            {
                TcpSocket.EndConnect(result);

                if (!TcpSocket.Connected) return;

                _stream = TcpSocket.GetStream();
                _receivedData = new Packet();
                _stream.BeginRead(_receiveBuffer, 0, DataBufferSize, TcpReceiveCallback, null);
            }
            
            public void TcpSendData(Packet packet)
            {
                try
                {
                    if (TcpSocket == null) return;

                    _stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
                }
                catch (Exception e)
                {
                    Debug.Log($"Error sending data to server via TCP: {e}");
                }
            }

            public void UdpSendData(Packet packet)
            {
                try
                {
                    packet.Insert(MyId);
                    if (UdpSocket == null) return;

                    UdpSocket.BeginSend(packet.ToArray(), packet.Length(), null, null);
                }
                catch
                {
                    // ignored
                }
            }

            private void TcpReceiveCallback(IAsyncResult result)
            {
                try
                {
                    var byteLength = _stream.EndRead(result);
                    if (byteLength <= 0) return; //TODO: DISCONNECT

                    var data = new byte[byteLength];
                    Array.Copy(_receiveBuffer, data, byteLength);
                    
                    _receivedData.Reset(TcpHandleData(data));
                    _stream.BeginRead(_receiveBuffer, 0, DataBufferSize, TcpReceiveCallback, null);
                }
                catch
                {
                    //TODO DISCONNECT
                    // ignored
                }
            }
            
            private void UdpReceiveCallback(IAsyncResult result)
            {
                try
                {
                    var data = UdpSocket.EndReceive(result, ref ClientEndPoint);
                    UdpSocket.BeginReceive(UdpReceiveCallback, null);

                    if (data.Length < 4) return;

                    UdpHandleData(data);
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

                while (packetLength > 0 && packetLength <= _receivedData.UnreadLength())
                {
                    var packetBytes = _receivedData.ReadBytes(packetLength);
                    MainThreadScheduler.EnqueueOnMainThread(() =>
                    {
                        using (var packet = new Packet(packetBytes))
                        {
                            var packetId = packet.ReadInt();
                            _packetHandlers[packetId](packet);
                        }
                    });
                    
                    packetLength = 0;
                    if (_receivedData.UnreadLength() < sizeof(int)) continue;
                    packetLength = _receivedData.ReadInt();
                    if (packetLength <= 0) return true;
                }

                return packetLength <= 1;
            }
            
            private void UdpHandleData(byte[] data)
            {
                using (var packet = new Packet(data))
                {
                    var packetLength = packet.ReadInt();
                    data = packet.ReadBytes(packetLength);
                }
                
                MainThreadScheduler.EnqueueOnMainThread(() =>
                {
                    using (var packet = new Packet(data))
                    {
                        var packetId = packet.ReadInt();
                        _packetHandlers[packetId](packet);
                    }
                });
            }
        }

        private static void InitializeClientData()
        {
            _packetHandlers = new Dictionary<int, PacketHandler>()
            {
                {(int)ServerPackets.Welcome, ClientHandle.Welcome},
            };
        }
    }
}