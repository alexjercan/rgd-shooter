using System;
using System.Collections.Generic;
using System.Numerics;
using _Project.Scripts.Networking.ByteArray;
using _Project.Scripts.Networking.TCP;
using _Project.Scripts.Networking.UDP;

namespace _Project.Scripts.Networking
{
    public class ServerManager
    {
        public event EventHandler<PlayerData> PlayerSpawnMessageReceived;
        public event EventHandler<PlayerInput> PlayerInputMessageReceived; 
        
        private readonly IServerManager _udpServerManager;
        private readonly IServerManager _tcpServerManager;
        
        private readonly List<PlayerData> _playerDataList = new List<PlayerData>();

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
                case MessageType.PlayerInput:
                    HandlePlayerInput(receivedMessage);
                    break;
                default:
                    return;
            }
        }
        
        private void HandleWelcomeAck(ByteArrayReader byteArrayReader)
        {
            var (clientId, clientUsername) = MessageTemplates.ReadWelcomeAck(byteArrayReader);

            foreach (var value in _playerDataList)
                _tcpServerManager.SendMessage(clientId, MessageTemplates.WriteSpawnPlayer(value));
            
            var playerData = new PlayerData(clientId, clientUsername, new Vector3(), new Quaternion());
            _playerDataList.Add(playerData);
            _tcpServerManager.BroadcastMessage(MessageTemplates.WriteSpawnPlayer(playerData));
            PlayerSpawnMessageReceived?.Invoke(this, playerData);
        }

        private void HandlePlayerInput(ByteArrayReader byteArrayReader)
        {
            var playerInput = MessageTemplates.ReadPlayerInput(byteArrayReader);
            
            PlayerInputMessageReceived?.Invoke(this, playerInput);
        }
    }
}