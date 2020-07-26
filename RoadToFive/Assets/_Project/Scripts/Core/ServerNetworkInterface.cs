using System;
using System.Collections.Generic;
using _Project.Scripts.ByteArray;
using _Project.Scripts.Networking;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Project.Scripts.Core
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

        private void ReadMessage(int clientId, ByteArrayReader receivedMessage)
        {
            var packetType = (MessageType)receivedMessage.ReadInt();

            switch (packetType)
            {
                case MessageType.Invalid:
                    break;
                case MessageType.Welcome:
                    break;
                case MessageType.WelcomeAck:
                    HandleWelcomeAck(clientId, receivedMessage);
                    break;
                case MessageType.SpawnPlayer:
                    break;
                case MessageType.PlayerInput:
                    HandlePlayerInput(clientId, receivedMessage);
                    break;
                case MessageType.PlayerDisconnect:
                    PlayerDisconnectHandler.Handle(clientId, _server, _players, receivedMessage);
                    break;
                case MessageType.ServerDisconnect:
                    break;
                case MessageType.PlayerMovement:
                    break;
                default:
                    return;
            }
        }

        private void HandleWelcomeAck(int clientId, ByteArrayReader byteArrayReader)
        {
            var username = MessageTemplates.ReadWelcomeAck(byteArrayReader);
           
            foreach (var pair in _players)
                _server.SendTcpMessage(clientId, MessageTemplates.WriteSpawnPlayer(pair.Key, pair.Value.Position, pair.Value.Rotation));

            var position = spawnPointTransform.position;
            var yRotation = spawnPointTransform.rotation.y;
            var rotation = Quaternion.AngleAxis(yRotation, Vector3.up);
            var instance = Instantiate(playerPrefab, position, rotation);
            _players.Add(clientId, instance);

            instance.ClientId = clientId;
            instance.Position = position;
            instance.Rotation = new Vector2(0, yRotation);

            _server.BroadcastTcp(MessageTemplates.WriteSpawnPlayer(clientId, instance.Position, instance.Rotation));
        }

        private void HandlePlayerInput(int clientId, ByteArrayReader byteArrayReader)
        {
            var (movementInput, rotation) = MessageTemplates.ReadPlayerInput(byteArrayReader);

            var playerToHandle = _players[clientId];
            playerToHandle.MovementInput = movementInput;
            playerToHandle.Rotation = rotation;
            
            _server.BroadcastUdp(MessageTemplates.WritePlayerMovement(clientId, playerToHandle.Position, playerToHandle.Rotation));
        }

        private void OnApplicationQuit()
        {
            if (_server == null) return;
            
            _server.BroadcastTcp(MessageTemplates.WriteServerDisconnect());
            _server.Stop();
        }
    }

    public static class PlayerDisconnectHandler
    {
        public static void Handle(int clientId, Server server, Dictionary<int, ServerPlayerManager> players, ByteArrayReader byteArrayReader)
        {
            var otherId = MessageTemplates.ReadPlayerDisconnect(byteArrayReader);

            if (otherId != clientId) return;
            
            server.RemoveClient(clientId);
            server.BroadcastTcp(MessageTemplates.WritePlayerDisconnect(clientId));

            Object.Destroy(players[clientId].gameObject);
            players.Remove(clientId);
        }
    }
}