using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts
{
    public class PlayerClient : MonoBehaviour
    {
        [SerializeField] private CharacterController characterController;

        private Vector2 _movementInput;
        private bool _shootInput;

        private void Awake()
        {
            if (!characterController) characterController = GetComponent<CharacterController>();
        }

        public void Movement(InputAction.CallbackContext context)
        {
            _movementInput = context.ReadValue<Vector2>();
        }
        
        public void Shoot(InputAction.CallbackContext context)
        {
            _shootInput = context.ReadValueAsButton();
            Debug.Log(_shootInput);
        }
    }
}