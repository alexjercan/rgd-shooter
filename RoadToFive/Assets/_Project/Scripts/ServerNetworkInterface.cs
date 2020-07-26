using System;
using System.Collections.Generic;
using _Project.Scripts.ByteArray;
using _Project.Scripts.Networking;
using UnityEngine;

namespace _Project.Scripts
{
    public class ServerNetworkInterface : MonoBehaviour
    {
        private const int Port = 26950;
        private const int MaxPlayers = 20;
        
        [SerializeField] private ServerPlayerManager playerPrefab;
        [SerializeField] private Transform spawnPointTransform;

        private Server _server;

        private readonly Dictionary<int, ServerPlayerManager> _players = new Dictionary<int, ServerPlayerManager>();

        private void Start()
        {
            _server = new Server(MaxPlayers, Port, ReadMessage);
            
            _server.Listen();
        }
        
        public void BroadcastPositionRotation(int playerId, Vector3 playerPosition, Vector2 playerRotation)
        {
            var playerData = new PlayerData(playerId, playerPosition, playerRotation);

            _server.BroadcastUdp(MessageTemplates.WritePlayerMovement(playerData));
        }

        private void ReadMessage(ByteArrayReader receivedMessage)
        {
            var packetType = (MessageType)receivedMessage.ReadInt();

            switch (packetType)
            {
                case MessageType.Invalid:
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
                case MessageType.PlayerMovement:
                    break;
                default:
                    return;
            }
        }

        private void HandleWelcomeAck(ByteArrayReader byteArrayReader)
        {
            var (clientId, _) = MessageTemplates.ReadWelcomeAck(byteArrayReader);
           
            foreach (var player in _players.Values)
                _server.SendTcpMessage(clientId, MessageTemplates.WriteSpawnPlayer(player.PlayerData));

            var position = spawnPointTransform.position;
            var yRotation = spawnPointTransform.rotation.y;
            var rotation = Quaternion.AngleAxis(yRotation, Vector3.up);
            var playerData = new PlayerData(clientId, position, new Vector2(0, yRotation));
            _server.BroadcastTcp(MessageTemplates.WriteSpawnPlayer(playerData));
            
            var instance = Instantiate(playerPrefab, position, rotation);
            instance.PlayerData = playerData;
            
            _players.Add(playerData.Id, instance);
        }

        private void HandlePlayerInput(ByteArrayReader byteArrayReader)
        {
            var playerInput = MessageTemplates.ReadPlayerInput(byteArrayReader);

            var playerId = playerInput.Id;
            var playerToHandle = _players[playerId];
            playerToHandle.PlayerMovementInput = playerInput.MovementInput;
            playerToHandle.PlayerRotation = playerInput.Rotation;
        }

        private void HandlePlayerDisconnect(ByteArrayReader byteArrayReader)
        {
            var playerId = MessageTemplates.ReadPlayerDisconnect(byteArrayReader);
            
            _server.RemoveClient(playerId);
            _server.BroadcastTcp(MessageTemplates.WritePlayerDisconnect(playerId));

            Destroy(_players[playerId].gameObject);
            _players.Remove(playerId);
        }
        
        private void OnApplicationQuit()
        {
            if (_server == null) return;
            
            _server.BroadcastTcp(MessageTemplates.WriteServerDisconnect());
            _server.Stop();
        }
    }
}