using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.ServerSide.Enemy;
using _Project.Scripts.ServerSide.Item;
using _Project.Scripts.ServerSide.Networking;
using _Project.Scripts.ServerSide.Player;
using _Project.Scripts.Util;
using UnityEngine;

namespace _Project.Scripts.ServerSide
{
    public class ServerManager : MonoBehaviour
    {
        public static ServerManager Instance;

        public Dictionary<int, ServerPlayerManager> playerManagers = new Dictionary<int, ServerPlayerManager>();
        public Dictionary<int, ItemSpawner> itemSpawners = new Dictionary<int, ItemSpawner>();
        public Dictionary<int, EnemyManager> enemyManagers = new Dictionary<int, EnemyManager>();
        
        public SpawnableItems spawnableItems;
        [SerializeField] private Transform spawnLocation;
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

        private void Start()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 30;
            Server.Start(20, 26950);
        }

        private void OnApplicationQuit()
        {
            Server.Stop();
        }

        public void SendIntoGame(int clientId, string username)
        {
            var player = Instantiate(playerPrefab, spawnLocation.position, Quaternion.identity);
            var playerManager = player.GetComponent<ServerPlayerManager>();
            playerManager.Initialize(clientId, username);
            playerManagers.Add(clientId, playerManager);

            //======================INITIALIZING PLAYERS ON ALL CLIENTS==========================
            
            foreach (var manager in playerManagers.Values.Where(manager => manager.Id != clientId))
                ServerSend.SpawnPlayer(clientId, manager);

            foreach(var manager in playerManagers.Values)
                ServerSend.SpawnPlayer(manager.Id, playerManager);

            //======================INITIALIZING SPAWNERS ON ALL CLIENTS====================
            
            foreach (var serverItemSpawner in itemSpawners.Values)
                ServerSend.CreateItemSpawner(clientId, serverItemSpawner.SpawnerId,
                    serverItemSpawner.Position, serverItemSpawner.HasItem, serverItemSpawner.itemScriptableObject.Id);

            //======================INITIALIZING INEVNTORY ON ALL CLIENTS====================
            
            foreach (var serverPlayerManager in playerManagers.Values.Where(manager => manager.Id != clientId))
                ServerSend.InitializeInventory(clientId, serverPlayerManager.Id, serverPlayerManager.playerInventory.GetWeapons());

            foreach (var manager in playerManagers.Values.Where(manager => manager.Id != clientId))
                ServerSend.HandWeaponUpdate(manager.Id, manager.playerInventory.GetHandWeaponIndex());

            //======================INITIALIZING ENEMIES ON ALL CLIENTS====================
            
            foreach (var enemyManager in enemyManagers.Values.Where(manager => manager.IsAlive)) 
                ServerSend.SpawnEnemy(clientId, enemyManager);

            foreach (var enemyManager in enemyManagers.Values) enemyManager.enemyAi.AddTarget(playerManager.transform);
        }

        public void DeSpawn(int clientId)
        {
            var player = playerManagers[clientId];
            foreach (var enemyManager in enemyManagers.Values) enemyManager.enemyAi.AddTarget(player.transform);
            
            playerManagers.Remove(clientId);
            Destroy(player.gameObject);
            
        }
    }
}