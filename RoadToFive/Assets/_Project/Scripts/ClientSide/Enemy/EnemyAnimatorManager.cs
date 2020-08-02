using System;
using _Project.Scripts.ServerSide.Enemy;
using UnityEngine;

namespace _Project.Scripts.ClientSide.Enemy
{
    public class EnemyAnimatorManager : MonoBehaviour
    {
        public Animator animator;
        
        public void SetAiState(EnemyAi.AIState aiState)
        {
            switch (aiState)
            {
                case EnemyAi.AIState.idle:
                    //animator.SetBool("Chase", false);
                    //animator.SetBool("Attack", false);
                    break;
                case EnemyAi.AIState.chasing:
                    //animator.SetBool("Chase", true);
                    //animator.SetBool("Attack", false);
                    break;
                case EnemyAi.AIState.attack:
                    //animator.SetBool("Attack", true);
                    //animator.SetBool("Chase", false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(aiState), aiState, null);
            }
        }
    }
}