using _Project.Scripts.Mechanics;
using UnityEngine;

namespace _Project.Scripts.ServerSide.Player
{
    /// <summary>
    /// COMPONENTA DE PE PLAYER LA NIVEL DE SERVER CARE SE OCUPA DE MOVEMENT
    /// </summary>
    public class PlayerMovement : MonoBehaviour //DONE DO NOT MODIFY
    {
        public new Transform transform;
        
        [SerializeField] private CharacterController characterController;
        [SerializeField] private float movementSpeed = 6.0f;
        [SerializeField] private float jumpHeight = 1.0f;
        
        private Vector3 _movementInput;
        private CharacterMovement _characterMovement;

        private void Start() => _characterMovement = new CharacterMovement();

        private void FixedUpdate()
        {
            MoveCharacter();
        }
        
        public void SetInput(Vector3 movementInput, Quaternion rotation)
        {
            _movementInput = movementInput;
            transform.rotation = rotation;
        }
        
        private void MoveCharacter()
        {
            var controllerInput = _characterMovement.GetControllerInput(_movementInput, transform.forward, transform.right, characterController.isGrounded, jumpHeight, movementSpeed);
            characterController.Move(controllerInput * Time.fixedDeltaTime);
        }
    }
}