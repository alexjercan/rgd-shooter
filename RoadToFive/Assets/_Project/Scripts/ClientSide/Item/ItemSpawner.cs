using UnityEngine;

namespace _Project.Scripts.ClientSide.Item
{
    public class ItemSpawner : MonoBehaviour
    {
        private int _spawnerId;
        private bool _hasItem;
        private int _itemId;
        private GameObject _itemGameObject;

        public void Initialize(int spawnerId, bool hasItem, int itemId, GameObject itemPrefab)
        {
            _spawnerId = spawnerId;
            _hasItem = hasItem;
            _itemId = itemId;
            var transformCache = transform;
            _itemGameObject = Instantiate(itemPrefab, transformCache.position, transformCache.rotation);
            _itemGameObject.SetActive(_hasItem);
        }

        public void SpawnItem()
        {
            _hasItem = true;
            
            _itemGameObject.SetActive(true);
        }

        public void DeleteItem()
        {
            _hasItem = false;
            
            _itemGameObject.SetActive(false);
        }
    }
}