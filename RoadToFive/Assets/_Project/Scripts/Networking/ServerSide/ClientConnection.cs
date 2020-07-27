﻿using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using _Project.Scripts.Threading;
using UnityEngine;

namespace _Project.Scripts.Networking.ServerSide
{
    public class ClientConnection
    {
        private const int DataBufferSize = 4096;

        public TransmissionControlProtocol Tcp { get; }
        public UserDatagramProtocol Udp { get; }
        public Player Player { get; set; }

        private readonly int _clientId;

        public ClientConnection(int clientId)
        {
            _clientId = clientId;
            Tcp = new TransmissionControlProtocol(clientId);
            Udp = new UserDatagramProtocol(clientId);
        }

        public class TransmissionControlProtocol
        {
            public TcpClient Socket { get; private set; }

            private readonly int _id;

            private Packet _receivedData;
            private NetworkStream _stream;
            private byte[] _receiveBuffer;

            public TransmissionControlProtocol(int id) => _id = id;

            public void Connect(TcpClient socket)
            {
                Socket = socket;
                Socket.ReceiveBufferSize = DataBufferSize;
                Socket.SendBufferSize = DataBufferSize;

                _stream = Socket.GetStream();

                _receivedData = new Packet();
                _receiveBuffer = new byte[DataBufferSize];

                _stream.BeginRead(_receiveBuffer, 0, DataBufferSize, ReceiveCallback, null);

                ServerSend.Welcome(_id, "welcome to the server");
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
                    Debug.Log($"Error sending data to player {_id} via TCP: {e}");
                }
            }

            private void ReceiveCallback(IAsyncResult result)
            {
                try
                {
                    var byteLength = _stream.EndRead(result);
                    if (byteLength <= 0) return;

                    var data = new byte[byteLength];
                    Array.Copy(_receiveBuffer, data, byteLength);

                    _receivedData.Reset(HandleData(data));
                    _stream.BeginRead(_receiveBuffer, 0, DataBufferSize, ReceiveCallback, null);
                }
                catch (Exception e)
                {
                    Debug.Log($"Error receiving TCP data: {e}");
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
                            Server.PacketHandlers[packetId](_id, packet);
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
            private readonly int _id;
            public IPEndPoint ClientEndPoint { get; private set; }

            public UserDatagramProtocol(int id) => _id = id;

            public void Connect(IPEndPoint ipEndPoint)
            {
                ClientEndPoint = ipEndPoint;
            }

            public void SendData(Packet packet)
            {
                try
                {
                    if (ClientEndPoint == null) return;

                    Server.UdpListener.BeginSend(packet.ToArray(), packet.Length(), ClientEndPoint, null, null);
                }
                catch (Exception e)
                {
                    Debug.Log($"Error sending data to {ClientEndPoint} via UDP: {e}");
                }
            }

            public void HandleData(Packet data)
            {
                var packetLength = data.Length();
                var packetBytes = data.ReadBytes(packetLength);

                MainThreadScheduler.EnqueueOnMainThread(() =>
                {
                    using (var packet = new Packet(packetBytes))
                    {
                        var packetId = packet.ReadInt();
                        Server.PacketHandlers[packetId](_id, packet);
                    }
                });
            }
        }
        
        public void SendIntoGame(string username)
        {
            Player = new Player(_clientId, username, new Vector3());

            foreach (var connection in Server.ClientConnections.Values.Where(connection => connection.Player != null && connection._clientId != _clientId))
                ServerSend.SpawnPlayer(_clientId, connection.Player);

            foreach (var connection in Server.ClientConnections.Values.Where(connection => connection.Player != null))
                ServerSend.SpawnPlayer(connection._clientId, Player);
        }
    }
}