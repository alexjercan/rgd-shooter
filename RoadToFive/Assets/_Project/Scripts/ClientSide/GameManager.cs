using _Project.Scripts.ClientSide.Enemy;
using _Project.Scripts.ClientSide.Item;
using _Project.Scripts.ClientSide.Networking;
using _Project.Scripts.ClientSide.Player;
using _Project.Scripts.Util;
using UnityEngine;

namespace _Project.Scripts.ClientSide
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public ItemSpawnerManager itemSpawnerManager;
        public PlayerSpawnerManager playerSpawnerManager;
        public SpawnableItems spawnableItems;
        public EnemySpawnerManager enemySpawnerManager;
        
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
    }
}