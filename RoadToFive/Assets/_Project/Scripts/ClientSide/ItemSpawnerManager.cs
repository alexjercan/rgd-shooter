using System.Collections.Generic;
using _Project.Scripts.ClientSide.Item;
using UnityEngine;

namespace _Project.Scripts.ClientSide
{
    public class ItemSpawnerManager : MonoBehaviour
    {
        private static readonly Dictionary<int, ItemSpawner> ItemSpawners = new Dictionary<int, ItemSpawner>();

        [SerializeField] private ItemSpawner itemSpawnerPrefab;

        public void CreateItemSpawner(int spawnerId, Vector3 position, bool hasItem)
        {
            var spawner = Instantiate(itemSpawnerPrefab, position, Quaternion.identity);
            spawner.Initialize(spawnerId, hasItem);

            ItemSpawners.Add(spawnerId, spawner);
        }
    }
}