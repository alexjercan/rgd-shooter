using UnityEngine;

namespace _Project.Scripts.ClientSide.Item
{
    public class ClientItemSpawner : MonoBehaviour
    {
        private int _spawnerId;
        private bool _hasItem;
        private int _itemId;

        public void Initialize(int spawnerId, bool hasItem, int itemId)
        {
            _spawnerId = spawnerId;
            _hasItem = hasItem;
            _itemId = itemId;
        }

        public void SpawnItem()
        {
            _hasItem = true;
            
            
        }
    }
}