using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.ServerSide.Item
{
    public class ServerItemSpawner : MonoBehaviour
    {
        public int SpawnerId { get; private set; }
        public bool HasItem { get; private set; }
        public Vector3 Position => _transform.position;
        
        public static readonly Dictionary<int, ServerItemSpawner> ItemSpawners = new Dictionary<int, ServerItemSpawner>();

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
            ItemSpawners.Add(SpawnerId, this);
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
        }

        private void ItemPickedUp(int byPlayer)
        {
            HasItem = false;

            StartCoroutine(SpawnItem());
        }
    }
}