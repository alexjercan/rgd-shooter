using System.Collections.Generic;
using _Project.Scripts.Util.Item;
using UnityEngine;

namespace _Project.Scripts.ClientSide.Item
{
    public class ItemSpawnerManager : MonoBehaviour
    {
        private static readonly Dictionary<int, ClientItemSpawner> ItemSpawners = new Dictionary<int, ClientItemSpawner>();

        [SerializeField] private List<ItemScriptableObject> itemScriptableObjects;
        [SerializeField] private ClientItemSpawner clientItemSpawnerPrefab;

        private readonly Dictionary<int, GameObject> _itemPrefabs = new Dictionary<int, GameObject>();
        
        private void Awake()
        {
            foreach (var itemScriptableObject in itemScriptableObjects)
                _itemPrefabs.Add(itemScriptableObject.id, itemScriptableObject.prefab);
        }

        public void CreateItemSpawner(int spawnerId, Vector3 position, bool hasItem, int itemId)
        {
            var spawner = Instantiate(clientItemSpawnerPrefab, position, Quaternion.identity);
            spawner.Initialize(spawnerId, hasItem, itemId, _itemPrefabs[itemId]);

            ItemSpawners.Add(spawnerId, spawner);
        }

        public void SpawnItem(int spawnerId) => ItemSpawners[spawnerId].SpawnItem();

        public void DeleteItem(int spawnerId) => ItemSpawners[spawnerId].DeleteItem();
    }
}