using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.ServerSide.Networking;
using UnityEngine;
using UnityEngine.AI;

public class enemy_ai : MonoBehaviour
{
    NavMeshAgent nm;
    public int enemyId;
    public Transform target;
    public List<Transform> targets = new List<Transform>();

    public enum AIState {idle, chasing, attack};

    public AIState aiState = AIState.idle;

    public float DistanceThreshold = 10f ;
    
    public float reactionTime = 0.2f;
    
    public float attackThreshold = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        nm = GetComponent<NavMeshAgent>();
        StartCoroutine(Think());
    }

    public void SetTargets(IEnumerable<Transform> targetTransforms) => targets.AddRange(targetTransforms);

    public void AddTarget(Transform targetTransform) => targets.Add(targetTransform);

    public void RemoveTarget(Transform targetTransform) => targets.Remove(targetTransform);

    IEnumerator Think() //used for optimization
    {
        while(true)
        {
            yield return new WaitForSeconds(reactionTime);
            
            if (!targets.Any()) continue;
            target = targets.First(); //TODO: AM FACUT CEVA CA SA MEARGA + mutat yield la inceput
            
            switch (aiState)
            {
                case AIState.idle:
                     float dist = Vector3.Distance(target.position, transform.position);
                    if (dist < DistanceThreshold)
                    {
                        aiState = AIState.chasing;
                        ServerSend.SendEnemyState(enemyId, (int) aiState);
                    }
                    nm.SetDestination(transform.position);
                    break;
                case AIState.chasing:
                    dist = Vector3.Distance(target.position, transform.position);
                    if (dist > DistanceThreshold)
                    {
                        aiState = AIState.idle;
                        ServerSend.SendEnemyState(enemyId, (int) aiState);
                    }
                    nm.SetDestination(target.position);
                    if(dist < attackThreshold)
                    {
                        aiState = AIState.attack;
                        ServerSend.SendEnemyState(enemyId, (int) aiState);
                    }
                    break;
                case AIState.attack:
                    //Debug.Log("Attack ! ");
                    dist = Vector3.Distance(target.position, transform.position);
                    if(dist> attackThreshold)
                    {
                        aiState = AIState.chasing;
                        ServerSend.SendEnemyState(enemyId, (int) aiState);
                    }

                    break;
                default:
                    break;
            }
        }
    }

}
