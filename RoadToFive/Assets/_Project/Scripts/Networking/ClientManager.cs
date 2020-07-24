using System;
using System.Collections.Generic;
using _Project.Scripts.Networking.ByteArray;
using _Project.Scripts.Networking.TCP;
using _Project.Scripts.Networking.UDP;

namespace _Project.Scripts.Networking
{
    public class ClientManager
    {
        public int Id { get; private set; }

        public event EventHandler<PlayerData> PlayerSpawnMessageReceived;
        
        private IClientManager _udpClientManager;
        private IClientManager _tcpClientManager;

        private readonly List<PlayerData> _playerDataList = new List<PlayerData>();

        public ClientManager()
        {
            Id = 0;
            _udpClientManager = new UdpClientManager(ReadMessage);
            _tcpClientManager = new TcpClientManager(ReadMessage);
        }

        public void SendMessage(byte[] message) => _udpClientManager.SendMessage(Id, message);

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
                case MessageType.PlayerInput:
                    break;
                default:
                    return;
            }
        }

        private void HandleDummy(ByteArrayReader byteArrayReader)
        {
            var id = MessageTemplates.ReadDummy(byteArrayReader);
            if (Id != id)
            {
            }
            //TODO: HANDLE ERROR/DISCONNECT 
        }
        
        private void HandleWelcome(ByteArrayReader byteArrayReader)
        {
            var (id, _) = MessageTemplates.ReadWelcome(byteArrayReader);
            Id = id;
            
            _udpClientManager.SendMessage(0, MessageTemplates.WriteDummy(id));
            _tcpClientManager.SendMessage(id, MessageTemplates.WriteWelcomeAck(id, "guest " + id));
        }

        private void HandleSpawnPlayer(ByteArrayReader byteArrayReader)
        {
            var playerData = MessageTemplates.ReadSpawnPlayer(byteArrayReader);

            _playerDataList.Add(playerData);
            PlayerSpawnMessageReceived?.Invoke(this, playerData);
        }
    }
}