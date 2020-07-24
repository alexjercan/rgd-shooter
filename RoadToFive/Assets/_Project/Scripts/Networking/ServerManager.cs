using System;
using System.Collections.Generic;
using System.Numerics;
using _Project.Scripts.Networking.ByteArray;
using _Project.Scripts.Networking.TCP;
using _Project.Scripts.Networking.UDP;
using Logger = _Project.Scripts.Logging.Logger;

namespace _Project.Scripts.Networking
{
    public class ServerManager
    {
        private readonly IServerManager _udpServerManager;
        private readonly IServerManager _tcpServerManager;
        
        private readonly Dictionary<int, Player> _players = new Dictionary<int, Player>();

        public ServerManager()
        {
            _udpServerManager = new UdpServerManager(ReadMessage);
            _tcpServerManager = new TcpServerManager(ReadMessage);
        }
        
        private void ReadMessage(ByteArrayReader receivedMessage)
        {
            var packetType = (MessageType)receivedMessage.ReadInt();

            switch (packetType)
            {
                case MessageType.Invalid:
                    break;
                case MessageType.Dummy:
                    break;
                case MessageType.DummyAck:
                    break;
                case MessageType.Welcome:
                    break;
                case MessageType.WelcomeAck:
                    HandleWelcomeAck(receivedMessage);
                    break;
                case MessageType.SpawnPlayer:
                    break;
                default:
                    return;
            }
        }
        
        private void HandleWelcomeAck(ByteArrayReader byteArrayReader)
        {
            var (clientId, clientUsername) = MessageTemplates.ReadWelcomeAck(byteArrayReader);
            
            Logger.Info($": received username: '{clientUsername}' of client {clientId}");

            foreach (var idPlayerPair in _players)
                _tcpServerManager.SendMessage(clientId,
                    MessageTemplates.WriteSpawnPlayer(idPlayerPair.Key, idPlayerPair.Value));
            
            var player = new Player(clientId, clientUsername, new Vector3(), new Quaternion());
            _players.Add(clientId, player);
            _tcpServerManager.BroadcastMessage(MessageTemplates.WriteSpawnPlayer(clientId, player));
        }
    }
}