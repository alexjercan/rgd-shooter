using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.ServerSide.Networking;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.ServerSide
{
    public class ServerManager : MonoBehaviour
    {
        public static ServerManager Instance;

        public Dictionary<int, ServerPlayerManager> playerManagers = new Dictionary<int, ServerPlayerManager>();

        [SerializeField] private string mainSceneName;
        [SerializeField] private Transform spawnLocation;
        [SerializeField] private GameObject playerPrefab;
        
        private AsyncOperation _asyncOperation;
        
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
            
            _asyncOperation = SceneManager.LoadSceneAsync(mainSceneName, LoadSceneMode.Additive);
            _asyncOperation.completed += operation => Server.Start(20, 26950);
        }

        private void Start()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 30;
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
        }

        public void DeSpawn(int clientId)
        {
            var player = playerManagers[clientId];
            playerManagers.Remove(clientId);
            Destroy(player.gameObject);
        }
    }
}