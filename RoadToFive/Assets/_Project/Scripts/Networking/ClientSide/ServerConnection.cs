using System;
using System.Net;
using System.Net.Sockets;
using _Project.Scripts.Threading;
using UnityEngine;

namespace _Project.Scripts.Networking.ClientSide
{
    public class ServerConnection
    {
        private const int DataBufferSize = 4096;

        public TransmissionControlProtocol Tcp { get; }
        public UserDatagramProtocol Udp { get; }

        public ServerConnection()
        {
            Tcp = new TransmissionControlProtocol();
            Udp = new UserDatagramProtocol();
        }

        public class TransmissionControlProtocol
        {
            public TcpClient Socket { get; set; }

            private Packet _receivedData;
            private NetworkStream _stream;
            private byte[] _receiveBuffer;

            public void Connect()
            {
                Socket = new TcpClient
                {
                    ReceiveBufferSize = DataBufferSize,
                    SendBufferSize = DataBufferSize
                };

                _receiveBuffer = new byte[DataBufferSize];
                Socket.BeginConnect(Client.ServerIp, Client.ServerPort, ConnectCallback, Socket);
            }

            public void SendData(Packet packet)
            {
                try
                {
                    if (Socket == null) return;

                    _stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
                }
                catch (Exception e)
                {
                    Debug.Log($"Error sending data to server via TCP: {e}");
                }
            }

            private void ConnectCallback(IAsyncResult result)
            {
                Socket.EndConnect(result);

                if (!Socket.Connected) return;

                _stream = Socket.GetStream();
                _receivedData = new Packet();
                _stream.BeginRead(_receiveBuffer, 0, DataBufferSize, ReceiveCallback, null);
            }

            private void ReceiveCallback(IAsyncResult result)
            {
                try
                {
                    var byteLength = _stream.EndRead(result);
                    if (byteLength <= 0) return; //TODO: DISCONNECT

                    var data = new byte[byteLength];
                    Array.Copy(_receiveBuffer, data, byteLength);

                    _receivedData.Reset(HandleData(data));
                    _stream.BeginRead(_receiveBuffer, 0, DataBufferSize, ReceiveCallback, null);
                }
                catch
                {
                    //DISCONNECT
                }
            }

            private bool HandleData(byte[] data)
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
                            Client.PacketHandlers[packetId](packet);
                        }
                    });

                    packetLength = 0;
                    if (_receivedData.UnreadLength() < sizeof(int)) break;
                    packetLength = _receivedData.ReadInt();
                    if (packetLength <= 0) return true;
                }

                return packetLength <= 1;
            }
        }

        public class UserDatagramProtocol
        {
            public UdpClient Socket { get; set; }

            public IPEndPoint ServerEndPoint
            {
                get => _serverEndPoint;
                set => _serverEndPoint = value;
            }

            private IPEndPoint _serverEndPoint;

            public UserDatagramProtocol()
            {
                _serverEndPoint = new IPEndPoint(IPAddress.Parse(Client.ServerIp), Client.ServerPort);
            }

            public void Connect(int localPort)
            {
                Socket = new UdpClient(localPort);

                Socket.Connect(ServerEndPoint);
                Socket.BeginReceive(ReceiveCallback, null);

                using (var packet = new Packet())
                {
                    SendData(packet);
                }
            }

            public void SendData(Packet packet)
            {
                try
                {
                    packet.Insert(Client.MyId);
                    if (Socket == null) return;

                    Socket.BeginSend(packet.ToArray(), packet.Length(), null, null);
                }
                catch (Exception e)
                {
                    Debug.Log($"Error sending data to server via UDP: {e}");
                }
            }

            private void ReceiveCallback(IAsyncResult result)
            {
                try
                {
                    var data = Socket.EndReceive(result, ref _serverEndPoint);
                    Socket.BeginReceive(ReceiveCallback, null);

                    if (data.Length < 4) return;

                    HandleData(data);
                }
                catch
                {
                    // TODO: DISCONNECT
                }
            }

            private void HandleData(byte[] data)
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
                        Client.PacketHandlers[packetId](packet);
                    }
                });
            }
        }
    }

}