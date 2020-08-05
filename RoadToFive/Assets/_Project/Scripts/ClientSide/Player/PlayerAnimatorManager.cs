using System;
using UnityEngine;

namespace _Project.Scripts.ClientSide.Player
{
    public class PlayerAnimatorManager : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Animator playerAnimator;

        private Vector3 _lastPosition;
        private Vector3 _velocity;

        private void Start()
        {
            _lastPosition = playerTransform.position;
        }

        private void Update()
        {
            var position = playerTransform.position;
            _velocity = position - _lastPosition;
            _lastPosition = position;
        }
    }
}