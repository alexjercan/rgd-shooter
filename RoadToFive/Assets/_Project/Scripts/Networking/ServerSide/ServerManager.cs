using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Scripts.Networking.ServerSide
{
    public class ServerManager : MonoBehaviour
    {
        public static ServerManager Instance;
        
        public Dictionary<int, PlayerManager> playerManagers = new Dictionary<int, PlayerManager>();
        
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
            Server.Start(20, 26950);
        }
        
        public void SendIntoGame(int clientId, string username)
        {
            var player = Instantiate(playerPrefab, new Vector3(), Quaternion.identity);
            var playerManager = player.GetComponent<PlayerManager>();
            playerManager.Initialize(clientId, username, new Vector3(), Quaternion.identity);
            playerManagers.Add(clientId, playerManager);

            foreach (var manager in playerManagers.Values.Where(manager => manager.Id != clientId))
                ServerSend.SpawnPlayer(clientId, manager);

            foreach(var manager in playerManagers.Values)
                ServerSend.SpawnPlayer(manager.Id, playerManager);
        }
    }
}