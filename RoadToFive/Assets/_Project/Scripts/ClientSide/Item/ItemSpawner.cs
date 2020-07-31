using UnityEngine;

namespace _Project.Scripts.ClientSide.Item
{
    public class ItemSpawner : MonoBehaviour
    {
        private int _spawnerId;
        private bool _hasItem;

        public void Initialize(int spawnerId, bool hasItem)
        {
            _spawnerId = spawnerId;
            _hasItem = hasItem;
        }
    }
}