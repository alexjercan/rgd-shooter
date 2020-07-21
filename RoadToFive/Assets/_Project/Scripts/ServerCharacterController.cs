using System;
using UnityEngine;

namespace _Project.Scripts
{
    public class ServerCharacterController : MonoBehaviour
    {
        public Vector2 MovementInput { get; set; }
        public bool JumpInput { get; set; }
        
        public Quaternion ClientRotation { get; set; }

        [SerializeField] private float speed = 12.0f;
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private float jumpHeight = 3.0f;

        [SerializeField] private CharacterController characterController;

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
            var movementInput = _transform.right * MovementInput.x + _transform.forward * MovementInput.y;
            
            _velocity.y = (float) (characterController.isGrounded
                ? (JumpInput ? Math.Sqrt(-2 * jumpHeight * gravity) : 0)
                : _velocity.y + gravity * Time.deltaTime);
            
            var controllerInput = movementInput * speed + _velocity;
            characterController.Move(controllerInput * Time.deltaTime);

            _transform.rotation = ClientRotation;
        }
    }
}