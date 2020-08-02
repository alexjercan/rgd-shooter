using System;
using System.Linq;
using _Project.Scripts.Mechanics;
using _Project.Scripts.ServerSide.Networking;
using _Project.Scripts.Util.Enemy;
using UnityEngine;

namespace _Project.Scripts.ServerSide.Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        public bool IsAlive { get; private set; }
        public int EnemyId { get; private set; }

        public EnemyScriptableObject enemyScriptableObject;
        public EnemyAi enemyAi;
        public EntityHealth entityHealth;
        public new Transform transform;
        
        private static int _nextEnemyId;
        
        private void Start()
        {
            IsAlive = true;
            ServerManager.Instance.enemyManagers.Add(_nextEnemyId, this);
            EnemyId = _nextEnemyId;
            _nextEnemyId++;

            enemyAi.enemyId = EnemyId;

            var transforms = ServerManager.Instance.playerManagers.Values.Select(manager => manager.transform);
            enemyAi.SetTargets(transforms);

            entityHealth.HealthChanged += (sender, health) => ServerSend.EnemyHealth(EnemyId, health);
            
            entityHealth.Died += (sender, args) =>
            {
                IsAlive = false;
                gameObject.SetActive(false);
            };
        }

        private void FixedUpdate()
        {
            ServerSend.EnemyPositionAndRotation(EnemyId, transform.position, transform.rotation);
        }
    }
}