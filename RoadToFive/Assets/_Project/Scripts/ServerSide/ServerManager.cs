﻿using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.ServerSide.Item;
using _Project.Scripts.ServerSide.Networking;
using _Project.Scripts.ServerSide.Player;
using UnityEngine;

namespace _Project.Scripts.ServerSide
{
    public class ServerManager : MonoBehaviour
    {
        public static ServerManager Instance;

        public Dictionary<int, ServerPlayerManager> playerManagers = new Dictionary<int, ServerPlayerManager>();

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

            foreach (var manager in playerManagers.Values.Where(manager => manager.Id != clientId))
                ServerSend.SpawnPlayer(clientId, manager);

            foreach(var manager in playerManagers.Values)
                ServerSend.SpawnPlayer(manager.Id, playerManager);

            foreach (var serverItemSpawner in ItemSpawnerManager.ItemSpawners.Values)
                ServerSend.CreateItemSpawner(clientId, serverItemSpawner.SpawnerId,
                    serverItemSpawner.Position, serverItemSpawner.HasItem, serverItemSpawner.ItemId);
        }

        public void DeSpawn(int clientId)
        {
            var player = playerManagers[clientId];
            playerManagers.Remove(clientId);
            Destroy(player.gameObject);
        }
    }
}