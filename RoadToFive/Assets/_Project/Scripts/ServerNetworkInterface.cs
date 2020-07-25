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
        [SerializeField] private PlayerManager playerPrefab;
        
        private UDP.ServerManager _udpServerManager;
        private TCP.ServerManager _tcpServerManager;
        
        private readonly List<PlayerData> _playerDataList = new List<PlayerData>();
        private readonly Dictionary<int, PlayerManager> _players = new Dictionary<int, PlayerManager>();

        private void Start()
        {
            _udpServerManager = new UDP.ServerManager(ReadMessage);
            _tcpServerManager = new TCP.ServerManager(ReadMessage);
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
                    //HandlePlayerInput(receivedMessage);
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
            var (clientId, clientUsername) = MessageTemplates.ReadWelcomeAck(byteArrayReader);

            foreach (var player in _players.Values)
                _tcpServerManager.SendMessage(clientId, MessageTemplates.WriteSpawnPlayer(player.PlayerData));
            
            var playerData = new PlayerData(clientId, clientUsername, new Numeric.Vector3(), new Numeric.Quaternion());
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
            playerToHandle.Rotate(playerInput.Rotation);
            playerToHandle.Move(playerInput.MovementInput);
            
            //TODO: SEND BACK RESULTS
        }

        private void HandlePlayerDisconnect(ByteArrayReader byteArrayReader)
        {
            var playerId = MessageTemplates.ReadPlayerDisconnect(byteArrayReader);
            
            _tcpServerManager.BroadcastMessageExcept(playerId, MessageTemplates.WritePlayerDisconnect(playerId));
            
            Destroy(_players[playerId]);
            _players.Remove(playerId);
        }
        
        private void OnApplicationQuit()
        {
            _tcpServerManager.BroadcastMessage(MessageTemplates.WriteServerDisconnect());
            _udpServerManager.Disconnect();
            _tcpServerManager.Disconnect();
        }
    }
}