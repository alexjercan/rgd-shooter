using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.ClientSide.Item
{
    public class ItemSpawnerManager : MonoBehaviour
    {
        private static readonly Dictionary<int, ClientItemSpawner> ItemSpawners = new Dictionary<int, ClientItemSpawner>();

        [SerializeField] private ClientItemSpawner clientItemSpawnerPrefab;

        public void CreateItemSpawner(int spawnerId, Vector3 position, bool hasItem, int itemId)
        {
            var spawner = Instantiate(clientItemSpawnerPrefab, position, Quaternion.identity);
            spawner.Initialize(spawnerId, hasItem, itemId);

            ItemSpawners.Add(spawnerId, spawner);
        }

        public void SpawnItem(int spawnerId)
        {
            ItemSpawners[spawnerId].SpawnItem();
        }
    }
}