using _Project.Scripts.Movement.Mechanics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Networking.ClientSide
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private Camera playerCamera;
        [SerializeField] private PlayerManager playerManager;

        [Header("Camera settings")]
        [Range(0.1f, 5.0f)] [SerializeField] private float mouseSensitivity = 2.0f;
        [Range(0.0f, 180.0f)] [SerializeField] private float verticalRotationRange = 170.0f;
        [Range(1.0f, 5.0f)] [SerializeField] private float cameraSmoothing = 1.0f;

        private Transform _transform;
        private Transform _cameraTransform;
        private float _internalMouseSensitivity;

        private LookRotation _characterLookRotation;
        
        private Vector2 _movementInput;
        private Vector2 _lookInput;
        private bool _jumpInput;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
            _cameraTransform = playerCamera.GetComponent<Transform>();
        }

        private void Start()
        {
            _internalMouseSensitivity = mouseSensitivity / 100.0f;
            var originalYRotation = _transform.eulerAngles.y;
            var baseCameraFieldOfView = playerCamera.fieldOfView;
            
            _characterLookRotation = new LookRotation(baseCameraFieldOfView, originalYRotation);
        }

        private void FixedUpdate()
        {
            RotateCharacter();
            
            ClientSend.PlayerMovement(MovementInput(_movementInput, _jumpInput), playerManager.Rotation);
        }
        
        public void MovementInputCallback(InputAction.CallbackContext context) => _movementInput = context.ReadValue<Vector2>();

        public void JumpInputCallback(InputAction.CallbackContext context) => _jumpInput = context.ReadValueAsButton();

        public void LookInputCallback(InputAction.CallbackContext context) => _lookInput = context.ReadValue<Vector2>();

        private static Vector3 MovementInput(Vector2 movementInput, bool jumpInput) => new Vector3(movementInput.x, jumpInput ? 1 : 0, movementInput.y);

        private void RotateCharacter()
        {
            var mouseYInput = _lookInput.y;
            var mouseXInput = _lookInput.x;
            var lookRotation = _characterLookRotation.GetCharacterRotation(mouseXInput, mouseYInput,
                playerCamera.fieldOfView, _internalMouseSensitivity, verticalRotationRange, cameraSmoothing);
            _cameraTransform.localRotation = Quaternion.Euler(lookRotation.x,0,0);
            playerManager.Rotation = Quaternion.Euler(0, lookRotation.y, 0);
        }
    }
}