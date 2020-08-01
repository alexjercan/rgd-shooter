using System.Linq;
using UnityEngine;

namespace _Project.Scripts.ServerSide.Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        public bool isAlive;
        public int enemyId;
        public enemy_ai enemyAi;

        private static int _enemyId;
        
        private void Start()
        {
            isAlive = true;
            ServerManager.Instance.enemyManagers.Add(_enemyId, this);
            enemyId = _enemyId;
            _enemyId++;

            var transforms = ServerManager.Instance.playerManagers.Values.Select(manager => manager.transform);
            enemyAi.SetTargets(transforms);
        }
    }
}