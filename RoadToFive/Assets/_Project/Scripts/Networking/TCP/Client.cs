using System;
using System.Net;
using System.Net.Sockets;
using _Project.Scripts.Logging;
using _Project.Scripts.Networking.ByteArray;
using _Project.Scripts.Networking.Packet;

namespace _Project.Scripts.Networking.TCP
{
    public class Client
    {
        private int _clientId;
        private readonly TransmissionControlProtocolSocket _tcpConnection;

        public int Port => ((IPEndPoint) _tcpConnection.Socket.Client.LocalEndPoint).Port;

        public Client(string serverIp, int port)
        {
            _tcpConnection = new TransmissionControlProtocolClient(this, serverIp, port);
            _tcpConnection.Connect(new TcpClient(new IPEndPoint(IPAddress.Any, 0)));
        }

        public void SendPacket(byte[] data) => _tcpConnection.SendPacket(data);

        public void ReadPacket(byte[] packet)
        {
            var packetReader = new ByteArrayReader(packet);
            var packetType = (ServerPacket)packetReader.ReadInt();

            switch (packetType)
            {
                case ServerPacket.InvalidPacket:
                    break;
                case ServerPacket.WelcomePacket:
                    HandleWelcomePacket(packetReader);
                    break;
                default:
                    return;
            }
        }
        
        private void HandleWelcomePacket(ByteArrayReader byteArrayReader)
        {
            var (clientId, message) = PacketTemplates.ReadWelcomePacket(byteArrayReader);

            _clientId = clientId;
            Logger.Info(message);
            SendPacket(PacketTemplates.WriteWelcomeReceivedPacket(_clientId, "guest " + clientId));
        }
    }
}