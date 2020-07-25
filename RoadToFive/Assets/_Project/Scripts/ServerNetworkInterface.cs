using System;
using System.Collections.Generic;
using _Project.Scripts.Networking;
using _Project.Scripts.Networking.ByteArray;
using UnityEngine;
using Numeric = System.Numerics;
using UDP = _Project.Scripts.Networking.UDP;
using TCP = _Project.Scripts.Networking.TCP;

namespace _Project.Scripts
{
    public class ServerNetworkInterface : MonoBehaviour
    {
        [SerializeField] private ServerPlayerManager playerPrefab;
        
        private UDP.ServerManager _udpServerManager;
        private TCP.ServerManager _tcpServerManager;
        
        private readonly List<PlayerData> _playerDataList = new List<PlayerData>();
        private readonly Dictionary<int, ServerPlayerManager> _players = new Dictionary<int, ServerPlayerManager>();

        private void Start()
        {
            _udpServerManager = new UDP.ServerManager(ReadMessage);
            _tcpServerManager = new TCP.ServerManager(ReadMessage);
        }
        
        public void BroadcastPositionRotation(int playerId, Vector3 playerPosition, Vector2 playerRotation)
        {
            var playerData = new PlayerData(playerId,
                new Numeric.Vector3(playerPosition.x, playerPosition.y, playerPosition.z),
                new Numeric.Vector2(playerRotation.x, playerRotation.y));
            
            _udpServerManager.BroadcastMessage(MessageTemplates.WritePlayerMovement(playerData));
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
                case MessageType.PlayerDisconnect:
                    HandlePlayerDisconnect(receivedMessage);
                    break;
                case MessageType.ServerDisconnect:
                    break;
                default:
                    return;
            }
        }

        private void HandleWelcomeAck(ByteArrayReader byteArrayReader)
        {
            var (clientId, _) = MessageTemplates.ReadWelcomeAck(byteArrayReader);

            foreach (var player in _players.Values)
                _tcpServerManager.SendMessage(clientId, MessageTemplates.WriteSpawnPlayer(player.PlayerData));
            
            var playerData = new PlayerData(clientId, new Numeric.Vector3(), new Numeric.Vector2());
            _tcpServerManager.BroadcastMessage(MessageTemplates.WriteSpawnPlayer(playerData));
            
            var instance = Instantiate(playerPrefab);
            instance.PlayerData = playerData;
            
            _players.Add(playerData.Id, instance);
        }

        private void HandlePlayerInput(ByteArrayReader byteArrayReader)
        {
            var playerInput = MessageTemplates.ReadPlayerInput(byteArrayReader);
            
            var playerId = playerInput.Id;
            var playerToHandle = _players[playerId];
            var rotationValue = playerInput.Rotation;
            var movementInput = playerInput.MovementInput;
            
            playerToHandle.PlayerMovementInput = new Vector3(movementInput.X, movementInput.Y, movementInput.Z);
            playerToHandle.PlayerRotation = new Vector2(rotationValue.X, rotationValue.Y);
        }

        private void HandlePlayerDisconnect(ByteArrayReader byteArrayReader)
        {
            var playerId = MessageTemplates.ReadPlayerDisconnect(byteArrayReader);
            
            _tcpServerManager.BroadcastMessageExcept(playerId, MessageTemplates.WritePlayerDisconnect(playerId));
            
            _udpServerManager.RemoveClient(playerId);
            _tcpServerManager.RemoveClient(playerId);
            
            Destroy(_players[playerId].gameObject);
            _players.Remove(playerId);
        }
        
        private void OnApplicationQuit()
        {
            if (_tcpServerManager == null || _udpServerManager == null) return;
            
            _tcpServerManager.BroadcastMessage(MessageTemplates.WriteServerDisconnect());
            _udpServerManager.Disconnect();
            _tcpServerManager.Disconnect();
        }
    }
}