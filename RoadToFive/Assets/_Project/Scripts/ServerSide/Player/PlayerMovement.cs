using _Project.Scripts.Mechanics;
using UnityEngine;

namespace _Project.Scripts.ServerSide.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public Transform PlayerTransform { get; private set; }
        
        [SerializeField] private CharacterController characterController;
        [SerializeField] private float movementSpeed = 6.0f;
        [SerializeField] private float jumpHeight = 1.0f;
        
        private Vector3 _movementInput;
        private CharacterMovement _characterMovement;
        
        private void Awake() => PlayerTransform = GetComponent<Transform>();
        
        private void Start() => _characterMovement = new CharacterMovement();

        private void FixedUpdate()
        {
            MoveCharacter();
        }
        
        public void SetInput(Vector3 movementInput, Quaternion rotation)
        {
            _movementInput = movementInput;
            PlayerTransform.rotation = rotation;
        }
        
        private void MoveCharacter()
        {
            var controllerInput = _characterMovement.GetControllerInput(_movementInput, PlayerTransform.forward, PlayerTransform.right, characterController.isGrounded, jumpHeight, movementSpeed);
            characterController.Move(controllerInput * Time.fixedDeltaTime);
        }
    }
}