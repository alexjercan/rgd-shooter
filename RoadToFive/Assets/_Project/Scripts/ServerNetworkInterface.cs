using System.Collections.Generic;
using _Project.Scripts.Networking;
using UnityEngine;
using Logger = _Project.Scripts.Logging.Logger;

namespace _Project.Scripts
{
    public class ServerNetworkInterface : MonoBehaviour
    {
        [SerializeField] private PlayerManager playerPrefab;
        
        private Dictionary<int, PlayerManager> _players = new Dictionary<int, PlayerManager>();
        
        private ServerManager _serverManager;
        
        private void Awake()
        {
            if (GetComponents<ServerNetworkInterface>().Length > 1) 
                Logger.Error("Multiple ServerManager instances in the scene!");
        }

        private void Start()
        {
            _serverManager = new ServerManager();
            _serverManager.PlayerSpawnMessageReceived += (sender, playerData) => SpawnPlayer(playerData);
            _serverManager.PlayerInputMessageReceived += (sender, playerInput) => HandlePlayerInput(playerInput);
        }
        
        private void SpawnPlayer(PlayerData playerData)
        {
            var player = Instantiate(playerPrefab);
            player.PlayerData = playerData;
            
            _players.Add(playerData.Id, player);
        }

        private void HandlePlayerInput(PlayerInput playerInput)
        {
            var playerId = playerInput.Id;
            var playerToHandle = _players[playerId];
            playerToHandle.Rotate(playerInput.Rotation);
            playerToHandle.Move(playerInput.MovementInput);
            
            //TODO: SEND BACK RESULTS
        }
    }
}