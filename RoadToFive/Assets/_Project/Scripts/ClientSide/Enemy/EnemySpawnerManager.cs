﻿using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.ClientSide.Enemy
{
    public class EnemySpawnerManager : MonoBehaviour
    {
        [SerializeField] private  EnemyManager enemyPrefab;

        private readonly Dictionary<int, EnemyManager> _enemies = new Dictionary<int, EnemyManager>();

        public void SpawnEnemy(int enemyId, Vector3 position, Quaternion rotation)
        {
            var enemy = Instantiate(enemyPrefab, position, rotation);
            enemy.entityHealth.Died += (sender, args) => DeleteEnemy(enemyId);

            _enemies.Add(enemyId, enemy);
        }

        public EnemyManager GetEnemy(int enemyId) => _enemies[enemyId];

        private void DeleteEnemy(int enemyId)
        {
            var enemyGameObject = _enemies[enemyId];
            _enemies.Remove(enemyId);
            enemyGameObject.gameObject.SetActive(false);
        }
    }
}