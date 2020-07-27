using _Project.Scripts.Movement.Mechanics;
using UnityEngine;

namespace _Project.Scripts.Movement.Character
{
    public class ServerCharacterController : MonoBehaviour
    {
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

        private void FixedUpdate()
        {
            UpdateRotation();
            
            MoveCharacter();
        }

        private void UpdateRotation()
        {
           //var rotationYValue = serverPlayerManager.Rotation.y;

            //_transform.localRotation = Quaternion.Euler(0, rotationYValue, 0);
        }

        private void MoveCharacter()
        {
            //var movementInput = serverPlayerManager.MovementInput;

            //var controllerInput = _characterMovement.GetControllerInput(movementInput, _transform.forward,
            //    _transform.right, characterController.isGrounded, jumpHeight, movementSpeed);

            //characterController.Move(controllerInput * Time.fixedDeltaTime);

            //serverPlayerManager.Position = _transform.position;
        }
    }
}