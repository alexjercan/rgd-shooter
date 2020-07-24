using System.Collections.Generic;
using _Project.Scripts.Networking;
using UnityEngine;
using Logger = _Project.Scripts.Logging.Logger;

namespace _Project.Scripts
{
    public class ClientNetworkInterface : MonoBehaviour
    {
        public PlayerManager LocalPlayer => _players[_clientManager.Id];

        [SerializeField] private PlayerManager localPlayerPrefab;
        [SerializeField] private PlayerManager playerPrefab;

        private Dictionary<int, PlayerManager> _players = new Dictionary<int, PlayerManager>();

        private ClientManager _clientManager;
        
        private void Awake()
        {
            if (GetComponents<ClientNetworkInterface>().Length > 1) 
                Logger.Error("Multiple ClientNetworkInterface instances in the scene!");
        }

        private void Start()
        {
            _clientManager = new ClientManager();
            _clientManager.PlayerSpawnMessageReceived += (sender, playerData) => SpawnPlayer(playerData);
        }

        private void SpawnPlayer(PlayerData playerData)
        {
            var player = Instantiate(_clientManager.Id == playerData.Id ? localPlayerPrefab : playerPrefab);

            player.PlayerData = playerData;
            
            _players.Add(playerData.Id, player);
        }
    }
}