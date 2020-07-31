using System.Collections;
using _Project.Scripts.ServerSide.Player;
using UnityEngine;

namespace _Project.Scripts.ServerSide.Item
{
    public class ServerItemSpawner : MonoBehaviour
    {
        public int SpawnerId { get; private set; }
        public int ItemId => itemId;
        public bool HasItem { get; private set; }
        public Vector3 Position => _transform.position;
        
        [SerializeField] private float spawnerTimer;
        [SerializeField] private int itemId;

        private static int _nextSpawnerId = 1;
        private Transform _transform;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        private void Start()
        {
            SpawnerId = _nextSpawnerId;
            ItemSpawnerManager.ItemSpawners.Add(SpawnerId, this);
            HasItem = false;
            
            _nextSpawnerId++;
            StartCoroutine(SpawnItem());
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!HasItem) return;
            if (!other.CompareTag("Player")) return;
            
            var playerManager = other.GetComponent<ServerPlayerManager>();
            ItemPickedUp(playerManager.Id);
        }

        private IEnumerator SpawnItem()
        {
            yield return new WaitForSeconds(spawnerTimer);

            HasItem = true;
            ItemSpawnerManager.ItemSpawned(SpawnerId);
        }

        private void ItemPickedUp(int byPlayer)
        {
            HasItem = false;

            StartCoroutine(SpawnItem());
            ItemSpawnerManager.ItemPickedUp(SpawnerId, byPlayer);
        }
    }
}