using System.Collections.Generic;
using _Project.Scripts.ClientSide.Networking;
using UnityEngine;

namespace _Project.Scripts.ClientSide.Player
{
    public class PlayerSpawnerManager : MonoBehaviour
    {
        private readonly Dictionary<int, PlayerManager> _playerManagers = new Dictionary<int, PlayerManager>();
        
        [SerializeField] private GameObject localPlayerPrefab;
        [SerializeField] private GameObject playerPrefab;
        
        public void SpawnPlayer(int id, string username, Vector3 position, Quaternion rotation)
        {
            var player = Instantiate(id == Client.MyId ? localPlayerPrefab : playerPrefab, position, rotation);

            var playerManager = player.GetComponent<PlayerManager>();
            playerManager.Initialize(id, username);
            _playerManagers.Add(id, playerManager);
        }
        
        public void DeSpawn(int clientId)
        {
            var player = _playerManagers[clientId];
            _playerManagers.Remove(clientId);
            Destroy(player.gameObject);
        }

        public PlayerManager GetPlayerManager(int id) => _playerManagers[id];
    }
}