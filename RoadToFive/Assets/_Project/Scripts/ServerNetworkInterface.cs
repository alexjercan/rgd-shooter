using System;
using System.Collections.Generic;
using _Project.Scripts.Networking;
using _Project.Scripts.Networking.ByteArray;
using UnityEngine;
using Numeric = System.Numerics;

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
            var playerData = new PlayerData(playerId,
                new Numeric.Vector3(playerPosition.x, playerPosition.y, playerPosition.z),
                new Numeric.Vector2(playerRotation.x, playerRotation.y));
            
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
            Debug.Log($"received welcome ack from {clientId} spawning their character...");

            foreach (var player in _players.Values)
                _server.SendTcpMessage(clientId, MessageTemplates.WriteSpawnPlayer(player.PlayerData));

            var position = spawnPointTransform.position;
            var rotation = Quaternion.AngleAxis(spawnPointTransform.rotation.y, Vector3.up);
            var playerData = new PlayerData(clientId, new Numeric.Vector3(position.x, position.y, position.z),
                new Numeric.Vector2(0, rotation.y));
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
            var rotationValue = playerInput.Rotation;
            var movementInput = playerInput.MovementInput;
            
            playerToHandle.PlayerMovementInput = new Vector3(movementInput.X, movementInput.Y, movementInput.Z);
            playerToHandle.PlayerRotation = new Vector2(rotationValue.X, rotationValue.Y);
            
            Debug.Log($"received player input from {playerId}");
        }

        private void HandlePlayerDisconnect(ByteArrayReader byteArrayReader)
        {
            var playerId = MessageTemplates.ReadPlayerDisconnect(byteArrayReader);
            Debug.Log($"player {playerId} disconnected");
            
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