using System;
using System.Collections.Generic;
using _Project.Scripts.ByteArray;
using _Project.Scripts.Networking;
using _Project.Scripts.SynchronizationComponents;
using UnityEngine;

namespace _Project.Scripts.Core
{
    public class ClientNetworkInterface : MonoBehaviour
    {
        private const int RemotePort = 26950;
        
        [SerializeField] private NetworkTransform localPlayerPrefab;
        [SerializeField] private NetworkTransform playerPrefab;

        private Client _client;
        private int _clientId;

        private readonly Dictionary<int, NetworkTransform> _players = new Dictionary<int, NetworkTransform>();
        private readonly List<INetworkTransferable> _localPlayerTransferables = new List<INetworkTransferable>();

        private void FixedUpdate()
        {
            foreach (var transferable in _localPlayerTransferables)
            {
                var data = transferable.Serialize();
                _client.SendUdpMessage(_clientId, MessageTemplates.WriteByteMessage(transferable.Type, data));
            }
        }

        public void ConnectToServer(string remoteIp)
        {
            _client = new Client(remoteIp, RemotePort, ReadMessage);
            _client.Listen();
        }

        private void ReadMessage(int clientId, ByteArrayReader receiveMessage)
        {
            var packetType = (MessageType)receiveMessage.ReadInt();

            switch (packetType)
            {
                case MessageType.Invalid:
                    break;
                case MessageType.Welcome:
                    HandleWelcome(clientId, receiveMessage);
                    break;
                case MessageType.WelcomeAck:
                    break;
                case MessageType.SpawnPlayer:
                    HandleSpawnPlayer(clientId, receiveMessage);
                    break;
                case MessageType.PlayerInput:
                    break;
                case MessageType.PlayerDisconnect:
                    HandlePlayerDisconnect(clientId, receiveMessage);
                    break;
                case MessageType.ServerDisconnect:
                    HandleServerDisconnect(clientId, receiveMessage);
                    break;
                case MessageType.PlayerMovement:
                    HandlePlayerMovement(clientId, receiveMessage);
                    break;
                default:
                    return;
            }
        }
        
        private void HandleWelcome(int clientId, ByteArrayReader byteArrayReader)
        {
            _clientId = clientId;
            MessageTemplates.ReadWelcome(byteArrayReader);

            _client.SendUdpMessage(clientId, MessageTemplates.WriteDummy());
            _client.SendTcpMessage(clientId, MessageTemplates.WriteWelcomeAck("guest " + clientId));
        }

        private void HandleSpawnPlayer(int clientId, ByteArrayReader byteArrayReader)
        {
            var (otherId, position, rotation) = MessageTemplates.ReadSpawnPlayer(byteArrayReader);

            var playerRotation = Quaternion.AngleAxis(rotation.y, Vector3.up);
            
            var player = Instantiate(otherId == clientId ? localPlayerPrefab : playerPrefab, position, playerRotation);

            player.PlayerRotation = rotation;
            player.PlayerPosition = position;
            _players.Add(otherId, player);

            if (otherId == clientId) _localPlayerTransferables.AddRange(player.GetComponents<INetworkTransferable>());
        }
        
        private void HandlePlayerMovement(int clientId, ByteArrayReader receiveMessage)
        {
            var (otherId, position, rotation) = MessageTemplates.ReadPlayerMovement(receiveMessage);
            
            var playerToHandle = _players[otherId];
            playerToHandle.PlayerPosition = position;
            playerToHandle.PlayerRotation = rotation;
        }

        private void HandlePlayerDisconnect(int clientId, ByteArrayReader byteArrayReader)
        {
            var otherId = MessageTemplates.ReadPlayerDisconnect(byteArrayReader);

            Destroy(_players[otherId].gameObject);
            _players.Remove(otherId);
        }

        private void HandleServerDisconnect(int clientId, ByteArrayReader byteArrayReader)
        {
            MessageTemplates.ReadServerDisconnect(byteArrayReader);
            
            foreach (var player in _players.Values) Destroy(player.gameObject);
            _players.Clear();
            _client.Disconnect();
        }

        private void OnApplicationQuit()
        {
            if (_client == null) return;
            
            _client.SendTcpMessage(_clientId, MessageTemplates.WritePlayerDisconnect(_clientId));
            _client.Disconnect();
        }
    }
}
