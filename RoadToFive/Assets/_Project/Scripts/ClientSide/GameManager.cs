using System.Collections.Generic;
using _Project.Scripts.ClientSide.Item;
using _Project.Scripts.ClientSide.Networking;
using UnityEngine;

namespace _Project.Scripts.ClientSide
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        
        public Dictionary<int, PlayerManager> playerManagers = new Dictionary<int, PlayerManager>();

        [SerializeField] private ItemSpawnerManager itemSpawnerManager;
        
        [SerializeField] private GameObject localPlayerPrefab;
        [SerializeField] private GameObject playerPrefab;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Debug.Log("Instance already exists, destroying object!");
                Destroy(this);
            }
        }

        private void OnApplicationQuit()
        {
            Client.Disconnect();
        }

        public void SpawnPlayer(int id, string username, Vector3 position, Quaternion rotation)
        {
            var player = Instantiate(id == Client.MyId ? localPlayerPrefab : playerPrefab, position, rotation);

            var playerManager = player.GetComponent<PlayerManager>();
            playerManager.Initialize(id, username);
            playerManagers.Add(id, playerManager);
        }
        
        public void DeSpawn(int clientId)
        {
            var player = playerManagers[clientId];
            playerManagers.Remove(clientId);
            Destroy(player.gameObject);
        }

        public void CreateItemSpawner(int spawnerId, Vector3 position, bool hasItem, int itemId) => 
            itemSpawnerManager.CreateItemSpawner(spawnerId, position, hasItem, itemId);

        public void SpawnItem(int spawnerId) => itemSpawnerManager.SpawnItem(spawnerId);
    }
}