using System;
using System.Collections.Generic;
using _Project.Scripts.Networking;
using _Project.Scripts.Networking.ByteArray;
using UnityEngine;
using UDP = _Project.Scripts.Networking.UDP;
using TCP = _Project.Scripts.Networking.TCP;

namespace _Project.Scripts
{
    public class ClientNetworkInterface : MonoBehaviour
    {
        public PlayerManager LocalPlayer => _players[_id];

        [SerializeField] private PlayerManager localPlayerPrefab;
        [SerializeField] private PlayerManager playerPrefab;
        
        private int _id;
        
        private UDP.ClientManager _udpClientManager;
        private TCP.ClientManager _tcpClientManager;
        
        private Dictionary<int, PlayerManager> _players = new Dictionary<int, PlayerManager>();
        
        private void Start()
        {
            _udpClientManager = new UDP.ClientManager(ReadMessage);
            _tcpClientManager = new TCP.ClientManager(ReadMessage);
            
            _tcpClientManager.Connect();
        }
        
        private void ReadMessage(ByteArrayReader receiveMessage)
        {
            var packetType = (MessageType)receiveMessage.ReadInt();

            switch (packetType)
            {
                case MessageType.Invalid:
                    break;
                case MessageType.Dummy:
                    HandleDummy(receiveMessage);
                    break;
                case MessageType.DummyAck:
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
                    HandleServerDisconnect(receiveMessage);
                    break;
                default:
                    return;
            }
        }

        private void HandleDummy(ByteArrayReader byteArrayReader)
        {
            var id = MessageTemplates.ReadDummy(byteArrayReader);
            //TODO: HANDLE ERROR/DISCONNECT IF IDs DONT MATCH
        }

        private void HandleWelcome(ByteArrayReader byteArrayReader)
        {
            var (id, _) = MessageTemplates.ReadWelcome(byteArrayReader);

            _id = id;

            _udpClientManager.SendMessage(0, MessageTemplates.WriteDummy(id));
            _tcpClientManager.SendMessage(MessageTemplates.WriteWelcomeAck(id, "guest " + id));
        }

        private void HandleSpawnPlayer(ByteArrayReader byteArrayReader)
        {
            var playerData = MessageTemplates.ReadSpawnPlayer(byteArrayReader);
            var player = Instantiate(_id == playerData.Id ? localPlayerPrefab : playerPrefab);

            player.PlayerData = playerData;
            
            _players.Add(playerData.Id, player);
        }

        private void HandlePlayerDisconnect(ByteArrayReader byteArrayReader)
        {
            var playerId = MessageTemplates.ReadPlayerDisconnect(byteArrayReader);

            Destroy(_players[playerId].gameObject);
            _players.Remove(playerId);
        }

        private void HandleServerDisconnect(ByteArrayReader receiveMessage)
        {
            foreach (var player in _players.Values) Destroy(player.gameObject);
            _players.Clear();
            _udpClientManager.Disconnect();
            _tcpClientManager.Disconnect();
        }
        
        private void OnApplicationQuit()
        {
            _tcpClientManager.SendMessage(MessageTemplates.WritePlayerDisconnect(_id));
            _udpClientManager.Disconnect();
            _tcpClientManager.Disconnect();
        }
    }
}