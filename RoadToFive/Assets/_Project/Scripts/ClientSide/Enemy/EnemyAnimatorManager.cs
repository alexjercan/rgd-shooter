using System;
using _Project.Scripts.ServerSide.Enemy;
using UnityEngine;

namespace _Project.Scripts.ClientSide.Enemy
{
    public class EnemyAnimatorManager : MonoBehaviour
    {
        public Animator animator;
        
        //TODO: FIX ANIMATIONS NOT WORKING, MODELS DESYNC BECAUSE OF THEM
        //TODO: MAKE ATTACK TRIGGER NOT BOOL
        public void SetAiState(EnemyAi.AIState aiState)
        {
            switch (aiState)
            {
                case EnemyAi.AIState.idle:
                    animator.SetBool("Attack", false);
                    animator.SetBool("Chase", false);
                    break;
                case EnemyAi.AIState.chasing:
                    animator.SetBool("Attack", false);
                    animator.SetBool("Chase", true);
                    break;
                case EnemyAi.AIState.attack:
                    animator.SetBool("Chase", false);
                    animator.SetBool("Attack", true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(aiState), aiState, null);
            }
        }
    }
}