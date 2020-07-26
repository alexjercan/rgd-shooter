using System;
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

        private Client _client;

        private readonly Dictionary<int, ClientPlayerManager> _players = new Dictionary<int, ClientPlayerManager>();
        private readonly List<INetworkTransferable> _localPlayerTransferables = new List<INetworkTransferable>();

        private void FixedUpdate()
        {
            foreach (var transferable in _localPlayerTransferables)
            {
                var data = transferable.Serialize();
                _client.SendUdpMessage(_client.Id, MessageTemplates.WriteByteMessage(transferable.Type, _client.Id, data));
            }
        }

        public void ConnectToServer(string remoteIp)
        {
            _client = new Client(remoteIp, RemotePort, ReadMessage);
            
            _client.Listen();
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
            _client.Id = MessageTemplates.ReadWelcome(byteArrayReader);

            _client.SendUdpMessage(_client.Id, MessageTemplates.WriteDummy());
            _client.SendTcpMessage(MessageTemplates.WriteWelcomeAck(_client.Id, "guest " + _client.Id));
        }

        private void HandleSpawnPlayer(ByteArrayReader byteArrayReader)
        {
            var (playerId, position, rotation) = MessageTemplates.ReadSpawnPlayer(byteArrayReader);

            var playerRotation = Quaternion.AngleAxis(rotation.y, Vector3.up);
            
            var player = Instantiate(_client.Id == playerId ? localPlayerPrefab : playerPrefab, position, playerRotation);

            player.PlayerRotation = rotation;
            player.PlayerPosition = position;
            _players.Add(playerId, player);

            if (playerId == _client.Id) _localPlayerTransferables.AddRange(player.GetComponents<INetworkTransferable>());
        }
        
        private void HandlePlayerMovement(ByteArrayReader receiveMessage)
        {
            var (playerId, position, rotation) = MessageTemplates.ReadPlayerMovement(receiveMessage);
            
            var playerToHandle = _players[playerId];
            playerToHandle.PlayerPosition = position;
            playerToHandle.PlayerRotation = rotation;
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
            
            _client.SendTcpMessage(MessageTemplates.WritePlayerDisconnect(_client.Id));
            _client.Disconnect();
        }
    }
}
