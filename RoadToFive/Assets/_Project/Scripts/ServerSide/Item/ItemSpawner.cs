using System.Collections;
using _Project.Scripts.Util.Item;
using UnityEngine;

namespace _Project.Scripts.ServerSide.Item
{
    public class ItemSpawner : MonoBehaviour  //DONE DO NOT MODIFY
    {
        public int SpawnerId { get; private set; }
        public bool HasItem { get; private set; }
        public Vector3 Position => _transform.position;
        
        [SerializeField] public ItemScriptableObject itemScriptableObject;
        [SerializeField] private float spawnerTimer;

        private static int _nextSpawnerId = 1;
        private Transform _transform;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        private void Start()
        {
            SpawnerId = _nextSpawnerId;
            HasItem = false;
            ServerManager.Instance.itemSpawners.Add(SpawnerId, this);
            
            _nextSpawnerId++;
            StartCoroutine(SpawnItem());
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!HasItem) return;

            TryPickUpItem(other);
        }

        private IEnumerator SpawnItem()
        {
            yield return new WaitForSeconds(spawnerTimer);

            HasItem = true;
            ItemSpawnerManager.OnItemSpawned(SpawnerId);
        }

        private void TryPickUpItem(Collider other)
        {
            HasItem = !ItemSpawnerManager.OnTryPickUpItem(SpawnerId, other);
            if (HasItem) return;
            StartCoroutine(SpawnItem());
        }
    }
}