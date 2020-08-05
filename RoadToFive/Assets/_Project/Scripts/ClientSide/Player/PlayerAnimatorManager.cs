using UnityEngine;

namespace _Project.Scripts.ClientSide.Player
{
    public class PlayerAnimatorManager : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Animator playerAnimator;

        private Vector3 _lastPosition;
        private Vector3 _velocity;
        private static readonly int Walk = Animator.StringToHash("Walk");
        private static readonly int Backwards = Animator.StringToHash("Backwards");

        private void Start()
        {
            _lastPosition = playerTransform.position;
        }

        private void Update()
        {
            var position = playerTransform.position;
            _velocity = position - _lastPosition;
            _lastPosition = position;

            UpdateAnimation();
        }

        private void UpdateAnimation()
        {
            if (_velocity == Vector3.zero)
            {
                playerAnimator.SetBool(Walk, false);
                return;
            }

            playerAnimator.SetBool(Walk, true);
            playerAnimator.SetBool(Backwards, !(Vector3.Angle(playerTransform.forward, _velocity) <= 180));
        }
    }
}