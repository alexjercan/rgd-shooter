using System.Collections.Generic;
using _Project.Scripts.Util.Enemy;
using UnityEngine;

namespace _Project.Scripts.Util
{
    public class SpawnableEnemies : MonoBehaviour
    {
        [SerializeField] public List<EnemyScriptableObject> items = new List<EnemyScriptableObject>();
        
        public List<EnemyScriptableObject> GetSpawnableItems() => items;

        private void Start()
        {
            for (var i = 0; i < items.Count; i++) items[i].Id = i;
        }
    }
}