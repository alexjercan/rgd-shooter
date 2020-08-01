using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.ClientSide.Enemy
{
    public class EnemySpawnerManager : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;

        private readonly Dictionary<int, GameObject> _enemies = new Dictionary<int, GameObject>();

        public void SpawnEnemy(int enemyId, Vector3 position, Quaternion rotation)
        {
            var enemy = Instantiate(enemyPrefab, position, rotation);

            _enemies.Add(enemyId, enemy);
        }

        public void DeleteEnemy(int enemyId)
        {
            var enemyGameObject = _enemies[enemyId];
            _enemies.Remove(enemyId);
            enemyGameObject.SetActive(false);
        }
    }
}