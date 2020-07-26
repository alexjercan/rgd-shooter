using System.Collections.Generic;
using System.Net;
using _Project.Scripts.Networking;
using _Project.Scripts.Networking.ByteArray;
using UnityEngine;
using Numeric = System.Numerics;

namespace _Project.Scripts
{
    public class ClientNetworkInterface : MonoBehaviour
    {
        private const int RemotePort = 26950;
        
        [SerializeField] private ClientPlayerManager localPlayerPrefab;
        [SerializeField] private ClientPlayerManager playerPrefab;
        
        private int _id;
        private Client _client;

        private Dictionary<int, ClientPlayerManager> _players = new Dictionary<int, ClientPlayerManager>();
        
        public void ConnectToServer(string remoteIp)
        {
            _client = new Client(remoteIp, RemotePort, ReadMessage);
            
            _client.Listen();
        }

        public void SendMovementInput(Vector2 movementInput, bool jumpInput, Vector2 rotationValue)
        {
            var movement = new Numeric.Vector3(movementInput.x, jumpInput ? 1 : 0, movementInput.y);
            var rotation = new Numeric.Vector2(rotationValue.x, rotationValue.y);
            var playerInput = new PlayerInput(_id, movement, rotation);
            _client.SendUdpMessage(_id, MessageTemplates.WritePlayerInput(playerInput));
        }
        
        private void ReadMessage(ByteArrayReader receiveMessage)
        {
            var packetType = (MessageType)receiveMessage.ReadInt();

            switch (packetType)
            {
                case MessageType.Invalid:
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
                case MessageType.PlayerDisconnect:
                    HandlePlayerDisconnect(receiveMessage);
                    break;
                case MessageType.ServerDisconnect:
                    HandleServerDisconnect();
                    break;
                case MessageType.PlayerMovement:
                    HandlePlayerMovement(receiveMessage);
                    break;
                default:
                    return;
            }
        }
        
        private void HandleWelcome(ByteArrayReader byteArrayReader)
        {
            _id = MessageTemplates.ReadWelcome(byteArrayReader);
            Debug.Log($"received my id: {_id}");

            _client.SendUdpMessage(_id, MessageTemplates.WriteDummy());
            _client.SendTcpMessage(MessageTemplates.WriteWelcomeAck(_id, "guest " + _id));
        }

        private void HandleSpawnPlayer(ByteArrayReader byteArrayReader)
        {
            Debug.Log("spawning player");
            var playerData = MessageTemplates.ReadSpawnPlayer(byteArrayReader);
            
            var playerPosition = new Vector3(playerData.Position.X, playerData.Position.Y, playerData.Position.Z);
            var playerRotation = Quaternion.AngleAxis(playerData.Rotation.Y, Vector3.up);
            
            var player = Instantiate(_id == playerData.Id ? localPlayerPrefab : playerPrefab, playerPosition, playerRotation);
            
            player.PlayerRotation = new Vector2(playerData.Rotation.X, playerData.Rotation.Y);
            player.PlayerPosition = playerPosition;
            _players.Add(playerData.Id, player);
        }
        
        private void HandlePlayerMovement(ByteArrayReader receiveMessage)
        {
            Debug.Log("received movement updates");
            var playerData = MessageTemplates.ReadPlayerMovement(receiveMessage);
            
            var playerId = playerData.Id;
            var playerToHandle = _players[playerId];
            var receivedPosition = playerData.Position;
            var receivedRotation = playerData.Rotation;
            
            playerToHandle.PlayerPosition = new Vector3(receivedPosition.X, receivedPosition.Y, receivedPosition.Z);
            playerToHandle.PlayerRotation = new Vector2(receivedRotation.X, receivedPosition.Y);
        }

        private void HandlePlayerDisconnect(ByteArrayReader byteArrayReader)
        {
            var playerId = MessageTemplates.ReadPlayerDisconnect(byteArrayReader);
            Debug.Log($"player {playerId} left the game");

            Destroy(_players[playerId].gameObject);
            _players.Remove(playerId);
        }

        private void HandleServerDisconnect()
        {
            Debug.Log("server closed");
            
            foreach (var player in _players.Values) Destroy(player.gameObject);
            _players.Clear();
            _client.Disconnect();
        }

        private void OnApplicationQuit()
        {
            if (_client == null) return;
            
            _client.SendTcpMessage(MessageTemplates.WritePlayerDisconnect(_id));
            _client.Disconnect();
        }
    }
}
