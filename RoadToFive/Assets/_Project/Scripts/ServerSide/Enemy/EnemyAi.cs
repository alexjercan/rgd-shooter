using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Mechanics;
using _Project.Scripts.ServerSide.Networking;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.ServerSide.Enemy
{
    public class EnemyAi : MonoBehaviour
    {
        public NavMeshAgent agent;
        public int enemyId;
        public EntityHealth target;
        public List<EntityHealth> targets = new List<EntityHealth>();

        public enum AIState {idle, chasing, attack};

        public AIState aiState = AIState.idle;

        public float DistanceThreshold = 10f ;
    
        public float reactionTime = 0.1f;
        public float attackInterval = 5.0f; 
    
        public float attackThreshold = 1.5f;

        public int damageAmount = 50;

        private float _waitTime;
        
        // Start is called before the first frame update
        void Start()
        {
            _waitTime = attackInterval;
            StartCoroutine(Think());
        }

        public void SetTargets(IEnumerable<EntityHealth> targetTransforms)
        {
            targets.AddRange(targetTransforms);
        }

        public void AddTarget(EntityHealth targetTransform)
        {
            targets.Add(targetTransform);
        }

        public void RemoveTarget(EntityHealth targetTransform)
        {
            targets.Remove(targetTransform);
        }

        IEnumerator Think() //used for optimization
        {
            while(true)
            {
                yield return new WaitForSeconds(reactionTime);
            
                if (!targets.Any()) continue;
                target = targets.First(); //TODO: AM FACUT CEVA CA SA MEARGA + mutat yield la inceput

                _waitTime += reactionTime;
            
                switch (aiState)
                {
                    case AIState.idle:
                        float dist = Vector3.Distance(target.transform.position, transform.position);
                        if (dist < DistanceThreshold)
                        {
                            aiState = AIState.chasing;
                            ServerSend.SendEnemyState(enemyId, (int) aiState);
                        }
                        agent.SetDestination(transform.position);
                        break;
                    case AIState.chasing:
                        dist = Vector3.Distance(target.transform.position, transform.position);
                        if (dist > DistanceThreshold)
                        {
                            aiState = AIState.idle;
                            ServerSend.SendEnemyState(enemyId, (int) aiState);
                        }
                        agent.SetDestination(target.transform.position);
                        if(dist < attackThreshold && _waitTime >= attackInterval)
                        {
                            aiState = AIState.attack;
                            _waitTime = 0;
                            ServerSend.SendEnemyState(enemyId, (int) aiState);
                        }
                        break;
                    case AIState.attack:
                        aiState = AIState.chasing;
                        target.Damage(damageAmount);
                        ServerSend.SendEnemyState(enemyId, (int) aiState);
                        break;
                    default:
                        break;
                }
            }
        }

    }
}
