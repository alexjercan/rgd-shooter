using System;
using UnityEngine;

namespace _Project.Scripts
{
    public class ServerCharacterController : MonoBehaviour
    {
        public Vector2 MovementInput { get; set; }
        public bool JumpInput { get; set; }
        public float CameraYRotation { get; set; }

        [SerializeField] private float speed = 12.0f;
        [SerializeField] private float turnSmoothTime = 0.1f;
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private float jumpHeight = 3.0f;
        
        [SerializeField] private CharacterController characterController;
        [SerializeField] private NetworkManagerMock networkManager;
        
        private Vector3 _velocity;
        private Transform _transform;
        private float _currentTurnVelocity;

        private void Awake()
        {
            if (!characterController) characterController = GetComponent<CharacterController>();
            _transform = GetComponent<Transform>();
        }

        private void Update()
        {
            var movementInput = new Vector3(MovementInput.x, 0, MovementInput.y).normalized;
            var movementDirection = movementInput;
            
            if (movementInput.sqrMagnitude > 0)
            {
                var targetAngle = Mathf.Atan2(movementInput.x, movementInput.z) * Mathf.Rad2Deg + CameraYRotation;
                var angle = Mathf.SmoothDampAngle(_transform.eulerAngles.y, targetAngle, ref _currentTurnVelocity, turnSmoothTime);
                _transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

                movementDirection = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;
            }

            _velocity.y = (float) (characterController.isGrounded
                ? (JumpInput ? Math.Sqrt(-2 * jumpHeight * gravity) : 0)
                : _velocity.y + gravity * Time.deltaTime);
            
            
            var controllerInput = movementDirection * speed + _velocity;
            characterController.Move(controllerInput * Time.deltaTime);
        }
    }
}