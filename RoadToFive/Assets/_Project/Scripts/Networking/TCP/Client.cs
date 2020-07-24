﻿using System.Net.Sockets;
using _Project.Scripts.Networking.ByteArray;

namespace _Project.Scripts.Networking.TCP
{
    public class Client
    {
        private readonly TransmissionControlProtocolSocket _socket;

        public Client(string serverIp, int port) => _socket = new TransmissionControlProtocolClient(serverIp, port);

        public void Connect() => _socket.Connect(new TcpClient());

        public void SendPacket(byte[] data) => _socket.SendPacket(data);

        public void SetReceivedDatagramHandler(ReceivedHandler handler) => _socket.ReceivedPacket += (sender, reader) => handler(sender, reader);

        public delegate void ReceivedHandler(object sender, ByteArrayReader reader);
    }
}