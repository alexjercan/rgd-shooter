using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.ClientSide.Enemy
{
    public class EnemySpawnerManager : MonoBehaviour
    {
        private readonly Dictionary<int, EnemyManager> _enemies = new Dictionary<int, EnemyManager>();

        public void SpawnEnemy(int enemyId, int enemyType, Vector3 position, Quaternion rotation)
        {
            var enemyPrefab = GameManager.Instance.spawnableEnemies.GetSpawnableItems()[enemyType].clientPrefab;
            
            var enemy = Instantiate(enemyPrefab, position, rotation);
            _enemies.Add(enemyId, enemy);
            
            enemy.entityHealth.Died += (sender, args) => DeleteEnemy(enemyId);
        }

        public EnemyManager GetEnemy(int enemyId)
        {
            return _enemies.ContainsKey(enemyId) == false ? null : _enemies[enemyId];
        }

        private void DeleteEnemy(int enemyId)
        {
            var enemyGameObject = _enemies[enemyId];
            _enemies.Remove(enemyId);
            enemyGameObject.gameObject.SetActive(false);
        }
    }
}