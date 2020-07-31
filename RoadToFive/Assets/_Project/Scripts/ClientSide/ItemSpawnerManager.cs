using System.Collections.Generic;
using _Project.Scripts.ClientSide.Item;
using UnityEngine;

namespace _Project.Scripts.ClientSide
{
    public class ItemSpawnerManager : MonoBehaviour
    {
        private static readonly Dictionary<int, ClientItemSpawner> ItemSpawners = new Dictionary<int, ClientItemSpawner>();

        [SerializeField] private ClientItemSpawner clientItemSpawnerPrefab;

        public void CreateItemSpawner(int spawnerId, Vector3 position, bool hasItem)
        {
            var spawner = Instantiate(clientItemSpawnerPrefab, position, Quaternion.identity);
            spawner.Initialize(spawnerId, hasItem);

            ItemSpawners.Add(spawnerId, spawner);
        }
    }
}