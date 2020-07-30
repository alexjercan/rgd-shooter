using System.Collections.Generic;
using _Project.Scripts.ClientSide.Networking;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.ClientSide
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
            playerManager.Initialize(id, username);
            playerManagers.Add(id, playerManager);
        }
        
        public void DeSpawn(int clientId)
        {
            var player = playerManagers[clientId];
            playerManagers.Remove(clientId);
            Destroy(player.gameObject);
        }
    }
}