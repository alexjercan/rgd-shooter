using System;
using UnityEngine;

namespace _Project.Scripts.ClientSide.Enemy
{
    public class EnemyAnimatorManager : MonoBehaviour
    {
        public Animator animator;
        
        public void SetAIState(enemy_ai.AIState aiState)
        {
            switch (aiState)
            {
                case enemy_ai.AIState.idle:
                    animator.SetBool("Idle", true);
                    animator.SetBool("Chase", false);
                    animator.SetBool("Attack", false);
                    break;
                case enemy_ai.AIState.chasing:
                    animator.SetBool("Chase", true);
                    animator.SetBool("Idle", false);
                    animator.SetBool("Attack", false);
                    break;
                case enemy_ai.AIState.attack:
                    animator.SetBool("Attack", true);
                    animator.SetBool("Idle", false);
                    animator.SetBool("Chase", false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(aiState), aiState, null);
            }
        }
    }
}