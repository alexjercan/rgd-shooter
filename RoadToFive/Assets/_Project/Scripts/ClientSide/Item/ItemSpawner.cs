using _Project.Scripts.Util.Item;
using UnityEngine;

namespace _Project.Scripts.ClientSide.Item
{
    public class ItemSpawner : MonoBehaviour
    {
        private int _spawnerId;
        private bool _hasItem;
        private int _itemId;
        private GameObject _itemGameObject;
        private ItemScriptableObject _itemScriptableObject;

        public void Initialize(int spawnerId, bool hasItem, int itemId, ItemScriptableObject itemScriptableObject)
        {
            _spawnerId = spawnerId;
            _hasItem = hasItem;
            _itemId = itemId;
            var transformCache = transform;
            _itemScriptableObject = itemScriptableObject;
            _itemGameObject = Instantiate(itemScriptableObject.prefab, transformCache.position, transformCache.rotation);
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

        public ItemScriptableObject GetItemData() => _itemScriptableObject;
    }
}