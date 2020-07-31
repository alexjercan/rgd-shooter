using System.Collections.Generic;
using _Project.Scripts.ClientSide.Item;
using _Project.Scripts.ClientSide.Networking;
using _Project.Scripts.ClientSide.Player;
using _Project.Scripts.Util;
using _Project.Scripts.Util.Item;
using UnityEngine;

namespace _Project.Scripts.ClientSide
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [SerializeField] private ItemSpawnerManager itemSpawnerManager;
        [SerializeField] private PlayerSpawnerManager playerSpawnerManager;
        [SerializeField] private SpawnableItems spawnableItems;
        
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

        private void OnApplicationQuit() => Client.Disconnect();

        public void SpawnPlayer(int id, string username, Vector3 position, Quaternion rotation) => 
            playerSpawnerManager.SpawnPlayer(id, username, position, rotation);

        public void DeSpawn(int clientId) => playerSpawnerManager.DeSpawn(clientId);
        
        public PlayerManager GetPlayerManager(int id) => playerSpawnerManager.GetPlayerManager(id);

        public void CreateItemSpawner(int spawnerId, Vector3 position, bool hasItem, int itemId) => 
            itemSpawnerManager.CreateItemSpawner(spawnerId, position, hasItem, itemId);

        public void SpawnItem(int spawnerId) => itemSpawnerManager.SpawnItem(spawnerId);

        public void DeleteItem(int spawnerId) => itemSpawnerManager.DeleteItem(spawnerId);
        
        public List<ItemScriptableObject> GetSpawnableItems() => spawnableItems.GetSpawnableItems();
    }
}