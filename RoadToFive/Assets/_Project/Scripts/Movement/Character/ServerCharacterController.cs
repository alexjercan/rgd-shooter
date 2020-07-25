using _Project.Scripts.Movement.Mechanics;
using UnityEngine;

namespace _Project.Scripts.Movement.Character
{
    public class ServerCharacterController : MonoBehaviour
    {
        [SerializeField] private ServerPlayerManager serverPlayerManager;
        [SerializeField] private CharacterController characterController;

        [SerializeField] private float movementSpeed = 12.0f;
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private float jumpHeight = 3.0f;
        
        private Transform _transform;
        private CharacterMovement _characterMovement;
        private ServerNetworkInterface _serverNetworkInterface;

        private void Awake()
        {
            if (!characterController) characterController = GetComponent<CharacterController>();
            _transform = GetComponent<Transform>();
            _serverNetworkInterface = FindObjectOfType<ServerNetworkInterface>();
        }

        private void Start()
        {
            _characterMovement = new CharacterMovement(gravity);
        }

        private void FixedUpdate()
        {
            UpdateRotation();
            
            MoveCharacter();

            _serverNetworkInterface.BroadcastPositionRotation(serverPlayerManager.PlayerData.Id, _transform.position,
                serverPlayerManager.PlayerRotation);
        }

        private void UpdateRotation()
        {
            var rotationYValue = serverPlayerManager.PlayerRotation.y;

            _transform.localRotation = Quaternion.Euler(0, rotationYValue, 0);
        }

        private void MoveCharacter()
        {
            var movementInput = serverPlayerManager.PlayerMovementInput;

            var controllerInput = _characterMovement.GetControllerInput(movementInput, _transform.forward,
                _transform.right, characterController.isGrounded, jumpHeight, movementSpeed);

            characterController.Move(controllerInput * Time.fixedDeltaTime);
        }
    }
}