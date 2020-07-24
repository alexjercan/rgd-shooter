using System;
using System.Collections.Generic;
using _Project.Scripts.Networking.ByteArray;
using _Project.Scripts.Networking.TCP;
using _Project.Scripts.Networking.UDP;
using Logger = _Project.Scripts.Logging.Logger;

namespace _Project.Scripts.Networking
{
    public class ClientManager
    {
        private readonly IClientManager _udpClientManager;
        private IClientManager _tcpClientManager;
        
        private readonly Dictionary<int, Player> _players = new Dictionary<int, Player>();
        
        private Player _player;
        
        public ClientManager()
        {
            _udpClientManager = new UdpClientManager(ReadMessage);
        }
        
        private void ReadMessage(ByteArrayReader receiveMessage)
        {
            var packetType = (MessageType)receiveMessage.ReadInt();

            switch (packetType)
            {
                case MessageType.Invalid:
                    break;
                case MessageType.Dummy:
                    HandleDummy(receiveMessage);
                    break;
                case MessageType.DummyAck:
                    break;
                case MessageType.Welcome:
                    HandleWelcome(receiveMessage);
                    break;
                case MessageType.WelcomeAck:
                    break;
                case MessageType.SpawnPlayer:
                    HandleSpawnPlayer(receiveMessage);
                    break;
                default:
                    return;
            }
        }

        private void HandleDummy(ByteArrayReader byteArrayReader)
        {
            var clientId = MessageTemplates.ReadDummy(byteArrayReader);
            Logger.Info($": received my new client id '{clientId}' from server!");
            _udpClientManager.SetClientId(clientId);
            _tcpClientManager = new TcpClientManager(ReadMessage);
        }
        
        private void HandleWelcome(ByteArrayReader byteArrayReader)
        {
            var (clientId, message) = MessageTemplates.ReadWelcome(byteArrayReader);

            Logger.Info($": received '{message}' from server!");
            
            _tcpClientManager.SendMessage(MessageTemplates.WriteWelcomeAck(clientId, "guest " + clientId));
        }

        private void HandleSpawnPlayer(ByteArrayReader byteArrayReader)
        {
            var (clientId, player) = MessageTemplates.ReadSpawnPlayer(byteArrayReader);

            Logger.Info($"Received player {player.Name}");
            
            _players.Add(clientId, player);
        }
    }
}