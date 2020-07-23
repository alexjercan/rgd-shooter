using System;
using System.Net;
using System.Net.Sockets;
using _Project.Scripts.Logging;
using _Project.Scripts.Networking.ByteArray;
using _Project.Scripts.Networking.Packet;
using _Project.Scripts.Networking.TCP;

namespace _Project.Scripts.Networking
{
    public class ClientTcp
    {
        private int _clientId;
        private readonly string _serverIp;
        private readonly int _port;
        
        private TcpConnection _tcpConnection;

        public ClientTcp(string serverIp, int port)
        {
            _serverIp = serverIp;
            _port = port;
        }
        
        public int ConnectToServer()
        {
            _tcpConnection = new TcpConnectionClient(this, _serverIp, _port);
            _tcpConnection.Connect(new TcpClient());
            return ((IPEndPoint) _tcpConnection.Socket.Client.LocalEndPoint).Port;
        }

        private void SendPacket(byte[] data) => _tcpConnection.SendPacket(data);

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
            var (clientId, message) = new WelcomePacketReader(byteArrayReader).ReadPacket();

            _clientId = clientId;
            Logger.Info(message);
            SendPacket(new WelcomeReceivedWriter(_clientId, "guest " + clientId).WritePacket());
        }
    }
}