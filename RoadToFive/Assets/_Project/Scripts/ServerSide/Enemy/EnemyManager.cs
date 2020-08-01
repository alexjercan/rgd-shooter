using System.Linq;
using UnityEngine;

namespace _Project.Scripts.ServerSide.Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        public bool IsAlive { get; private set; }
        public int EnemyId { get; private set; }
        public enemy_ai enemyAi;

        private static int _enemyId;
        
        private void Start()
        {
            IsAlive = true;
            ServerManager.Instance.enemyManagers.Add(_enemyId, this);
            EnemyId = _enemyId;
            _enemyId++;

            var transforms = ServerManager.Instance.playerManagers.Values.Select(manager => manager.transform);
            enemyAi.SetTargets(transforms);
        }
    }
}