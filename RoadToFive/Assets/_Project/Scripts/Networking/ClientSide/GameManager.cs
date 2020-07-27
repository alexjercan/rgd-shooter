﻿using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Networking.ClientSide
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        
        public Dictionary<int, PlayerManager> playerManagers = new Dictionary<int, PlayerManager>();

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
            playerManager.Id = id;
            playerManager.Username = username;
            playerManagers.Add(id, playerManager);
        }
    }
}