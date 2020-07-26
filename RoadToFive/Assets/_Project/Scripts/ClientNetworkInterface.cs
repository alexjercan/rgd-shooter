using System.Collections.Generic;
using _Project.Scripts.ByteArray;
using _Project.Scripts.Networking;
using UnityEngine;

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
            var movement = new Vector3(movementInput.x, jumpInput ? 1 : 0, movementInput.y);
            var rotation = new Vector2(rotationValue.x, rotationValue.y);
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

            _client.SendUdpMessage(_id, MessageTemplates.WriteDummy());
            _client.SendTcpMessage(MessageTemplates.WriteWelcomeAck(_id, "guest " + _id));
        }

        private void HandleSpawnPlayer(ByteArrayReader byteArrayReader)
        {
            var playerData = MessageTemplates.ReadSpawnPlayer(byteArrayReader);
            
            var playerRotation = Quaternion.AngleAxis(playerData.Rotation.y, Vector3.up);
            
            var player = Instantiate(_id == playerData.Id ? localPlayerPrefab : playerPrefab, playerData.Position, playerRotation);

            player.PlayerRotation = playerData.Rotation;
            player.PlayerPosition = playerData.Position;
            _players.Add(playerData.Id, player);
        }
        
        private void HandlePlayerMovement(ByteArrayReader receiveMessage)
        {
            var playerData = MessageTemplates.ReadPlayerMovement(receiveMessage);
            
            var playerId = playerData.Id;
            var playerToHandle = _players[playerId];
            var receivedPosition = playerData.Position;
            var receivedRotation = playerData.Rotation;
            
            playerToHandle.PlayerPosition = receivedPosition;
            playerToHandle.PlayerRotation = receivedRotation;
        }

        private void HandlePlayerDisconnect(ByteArrayReader byteArrayReader)
        {
            var playerId = MessageTemplates.ReadPlayerDisconnect(byteArrayReader);

            Destroy(_players[playerId].gameObject);
            _players.Remove(playerId);
        }

        private void HandleServerDisconnect()
        {

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
