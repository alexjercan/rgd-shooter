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
                _server.SendTcpMessage(clientId, MessageTemplates.WriteSpawnPlayer(player.ClientId, player.Position, player.Rotation));

            var position = spawnPointTransform.position;
            var yRotation = spawnPointTransform.rotation.y;
            var rotation = Quaternion.AngleAxis(yRotation, Vector3.up);
            var instance = Instantiate(playerPrefab, position, rotation);
            _players.Add(clientId, instance);

            instance.ClientId = clientId;
            instance.Position = position;
            instance.Rotation = new Vector2(0, yRotation);

            _server.BroadcastTcp(MessageTemplates.WriteSpawnPlayer(instance.ClientId, instance.Position, instance.Rotation));
        }

        private void HandlePlayerInput(ByteArrayReader byteArrayReader)
        {
            var (playerId, movementInput, rotation) = MessageTemplates.ReadPlayerInput(byteArrayReader);

            var playerToHandle = _players[playerId];
            playerToHandle.MovementInput = movementInput;
            playerToHandle.Rotation = rotation;
            
            _server.BroadcastUdp(MessageTemplates.WritePlayerMovement(playerId, playerToHandle.Position, playerToHandle.Rotation));
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