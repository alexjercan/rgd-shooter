using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemy_ai : MonoBehaviour
{
    NavMeshAgent nm;
    public Transform target;

    public enum AIState {idle, chasing, attack};

    public AIState aiState = AIState.idle;

    public float DistanceThreshold = 10f ;
    
    public float reactionTime = 0.2f;

    public Animator animator;
    public float attackThreshold = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        nm = GetComponent<NavMeshAgent>();
        StartCoroutine(Think());
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Think() //used for optimization
    {
        while(true)
        {
            switch (aiState)
            {
                case AIState.idle:
                     float dist = Vector3.Distance(target.position, transform.position);
                    if (dist < DistanceThreshold)
                    {
                        aiState = AIState.chasing;
                        animator.SetBool("Chase", true);
                    }
                    nm.SetDestination(transform.position);
                    break;
                case AIState.chasing:
                    dist = Vector3.Distance(target.position, transform.position);
                    if (dist > DistanceThreshold)
                    {
                        aiState = AIState.idle;
                        animator.SetBool("Chase", false);
                    }
                    nm.SetDestination(target.position);
                    if(dist < attackThreshold)
                    {
                        aiState = AIState.attack;
                        animator.SetBool("Attack", true);
                    }
                    break;
                case AIState.attack:
                    Debug.Log("Attack ! ");
                    dist = Vector3.Distance(target.position, transform.position);
                    if(dist> attackThreshold)
                    {
                        aiState = AIState.chasing;
                        animator.SetBool("Attack", false);
                    }

                    break;
                default:
                    break;
            }

            
            yield return new WaitForSeconds(reactionTime);
        }
    }

}
