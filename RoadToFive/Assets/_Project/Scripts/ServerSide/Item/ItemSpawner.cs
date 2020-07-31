using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.ServerSide.Item
{
    public class ItemSpawner : MonoBehaviour
    {
        public int SpawnerId { get; private set; }

        [SerializeField] private float spawnerTimer;
        
        private static readonly Dictionary<int, ItemSpawner> ItemSpawners = new Dictionary<int, ItemSpawner>();
        private static int _nextSpawnerId = 1;
        private bool _hasItem;

        private void Start()
        {
            Initialize(_nextSpawnerId);
            _nextSpawnerId++;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            var playerManager = other.GetComponent<ServerPlayerManager>();
            ItemPickedUp(playerManager.Id);
        }

        private void Initialize(int spawnerId)
        {
            SpawnerId = spawnerId;
            _hasItem = false;
            
            StartCoroutine(SpawnItem());
        }

        private IEnumerator SpawnItem()
        {
            yield return new WaitForSeconds(spawnerTimer);

            _hasItem = true;
        }

        private void ItemPickedUp(int byPlayer)
        {
            _hasItem = false;

            StartCoroutine(SpawnItem());
        }
    }
}