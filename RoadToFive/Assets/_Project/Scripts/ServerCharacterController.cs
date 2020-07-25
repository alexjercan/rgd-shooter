using _Project.Scripts.Character;
using _Project.Scripts.Networking;
using UnityEngine;

namespace _Project.Scripts
{
    public class ServerCharacterController : MonoBehaviour
    {
        [SerializeField] private ServerInputHandler serverInputHandler;
        [SerializeField] private CharacterController characterController;

        [SerializeField] private float movementSpeed = 12.0f;
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private float jumpHeight = 3.0f;
        
        private Transform _transform;
        private CharacterMovement _characterMovement;

        private void Awake()
        {
            if (!characterController) characterController = GetComponent<CharacterController>();
            _transform = GetComponent<Transform>();
        }

        private void Start()
        {
            _characterMovement = new CharacterMovement(gravity);
        }

        private void Update()
        {
            UpdateRotation();
            
            MoveCharacter();
        }

        private void UpdateRotation()
        {
            var rotationYValue = serverInputHandler.ClientRotationYValue;

            _transform.localRotation = Quaternion.Euler(0, rotationYValue, 0);
        }

        private void MoveCharacter()
        {
            var movementInput = serverInputHandler.MovementInput;
            var jumpInput = serverInputHandler.JumpInput;

            var controllerInput = _characterMovement.GetControllerInput(movementInput, jumpInput, _transform.forward,
                _transform.right, characterController.isGrounded, jumpHeight, movementSpeed);

            characterController.Move(controllerInput * Time.deltaTime);
        }
    }
}