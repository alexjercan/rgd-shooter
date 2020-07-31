using System.Collections.Generic;
using _Project.Scripts.Util.Item;
using UnityEngine;

namespace _Project.Scripts.ClientSide.Item
{
    public class ItemSpawnerManager : MonoBehaviour //DONE DO NOT MODIFY
    {
        [SerializeField] private List<ItemScriptableObject> items;
        [SerializeField] private ItemSpawner itemSpawnerPrefab;

        private readonly Dictionary<int, ItemSpawner> _itemSpawners = new Dictionary<int, ItemSpawner>();
        
        private void Awake()
        {
            for (var i = 0; i < items.Count; i++) items[i].Id = i;
        }

        public void CreateItemSpawner(int spawnerId, Vector3 position, bool hasItem, int itemId)
        {
            var spawner = Instantiate(itemSpawnerPrefab, position, Quaternion.identity);
            spawner.Initialize(spawnerId, hasItem, itemId, items[itemId]);

            _itemSpawners.Add(spawnerId, spawner);
        }

        public void SpawnItem(int spawnerId) => _itemSpawners[spawnerId].SpawnItem();

        public void DeleteItem(int spawnerId) => _itemSpawners[spawnerId].DeleteItem();
    }
}