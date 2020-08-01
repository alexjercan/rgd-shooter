using System;
using System.Linq;
using _Project.Scripts.Mechanics;
using _Project.Scripts.ServerSide.Networking;
using UnityEngine;

namespace _Project.Scripts.ServerSide.Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        public bool IsAlive { get; private set; }
        public int EnemyId { get; private set; }
        public enemy_ai enemyAi;
        public EntityHealth entityHealth;
        public new Transform transform;
        
        private static int _enemyId;
        
        private void Start()
        {
            IsAlive = true;
            ServerManager.Instance.enemyManagers.Add(_enemyId, this);
            EnemyId = _enemyId;
            _enemyId++;

            var transforms = ServerManager.Instance.playerManagers.Values.Select(manager => manager.transform);
            enemyAi.SetTargets(transforms);

            entityHealth.HealthChanged += (sender, health) => ServerSend.EnemyHealth(_enemyId, health);
            
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