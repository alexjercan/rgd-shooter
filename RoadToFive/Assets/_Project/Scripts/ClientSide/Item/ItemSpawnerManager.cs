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

        private readonly Dictionary<int, ItemScriptableObject> _itemScriptableObjects = new Dictionary<int, ItemScriptableObject>();
        
        private void Awake()
        {
            foreach (var itemScriptableObject in itemScriptableObjects)
                _itemScriptableObjects.Add(itemScriptableObject.id, itemScriptableObject);
        }

        public void CreateItemSpawner(int spawnerId, Vector3 position, bool hasItem, int itemId)
        {
            var spawner = Instantiate(clientItemSpawnerPrefab, position, Quaternion.identity);
            spawner.Initialize(spawnerId, hasItem, itemId, _itemScriptableObjects[itemId].prefab);

            ItemSpawners.Add(spawnerId, spawner);
        }

        public void SpawnItem(int spawnerId) => ItemSpawners[spawnerId].SpawnItem();

        public void DeleteItem(int spawnerId) => ItemSpawners[spawnerId].DeleteItem();
    }
}