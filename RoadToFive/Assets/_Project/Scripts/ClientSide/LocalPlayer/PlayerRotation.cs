using _Project.Scripts.Mechanics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.ClientSide.LocalPlayer
{
    /// <summary>
    /// COMPONENTA SPECIFICA LOCAL PLAYERULUI CARE SE OCUPA DE ROTATIE
    /// </summary>
    public class PlayerRotation : MonoBehaviour
    {
        [SerializeField] private Camera playerCamera;
        
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
        }
        
        public void LookInputCallback(InputAction.CallbackContext context) => 
            _lookInput = context.ReadValue<Vector2>();
        
        public void ToggleCursor(InputAction.CallbackContext context)
        {
            Cursor.visible = !Cursor.visible;
            Cursor.lockState = Cursor.lockState == CursorLockMode.None ? CursorLockMode.Locked : CursorLockMode.None;
        }

        public Quaternion GetRotation() => _transform.rotation;

        public Vector3 GetForwardDirection() => _cameraTransform.forward;
        
        private void RotateCharacter()
        {
            var mouseYInput = _lookInput.y;
            var mouseXInput = _lookInput.x;
            var lookRotation = _characterLookRotation.GetCharacterRotation(mouseXInput, mouseYInput,
                playerCamera.fieldOfView, _internalMouseSensitivity, verticalRotationRange, cameraSmoothing);
            _cameraTransform.localRotation = Quaternion.Euler(lookRotation.x,0,0);
            _transform.rotation = Quaternion.Euler(0, lookRotation.y, 0);
        }
    }
}