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

        public ItemSpawnerManager itemSpawnerManager;
        public PlayerSpawnerManager playerSpawnerManager;
        public SpawnableItems spawnableItems;
        
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

        public void SpawnEnemy(int enemyId, Vector3 enemyPosition, Quaternion enemyRotation)
        {
            Debug.Log($"Spawning enemy {enemyId}");
        }
    }
}