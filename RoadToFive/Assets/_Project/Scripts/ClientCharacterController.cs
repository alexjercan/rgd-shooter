using _Project.Scripts.Character;
using _Project.Scripts.Networking;
using UnityEngine;

namespace _Project.Scripts
{
    public class ClientCharacterController : MonoBehaviour
    {
        [SerializeField] private ClientInputHandler clientInputHandler;
        [SerializeField] private Camera playerCamera;
        
        [Header("Camera settings")]
        [Range(0.1f, 5.0f)] [SerializeField] private float mouseSensitivity = 2.0f;
        [Range(0.0f, 180.0f)] [SerializeField] private float verticalRotationRange = 170.0f;
        [Range(1.0f, 5.0f)] [SerializeField] private float cameraSmoothing = 1.0f;
        
        private Transform _transform;
        private Transform _cameraTransform;
        private float _internalMouseSensitivity;

        private CharacterLookRotation _characterLookRotation;

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
            
            _characterLookRotation = new CharacterLookRotation(baseCameraFieldOfView, originalYRotation);
        }

        private void Update()
        {
            RotateCharacter();

            UpdatePosition();
        }

        private void RotateCharacter()
        {
            var mouseYInput = clientInputHandler.LookInput.y;
            var mouseXInput = clientInputHandler.LookInput.x;
            var lookRotation = _characterLookRotation.GetCharacterRotation(mouseXInput, mouseYInput,
                playerCamera.fieldOfView, _internalMouseSensitivity, verticalRotationRange, cameraSmoothing);
            _cameraTransform.localRotation = Quaternion.Euler(lookRotation.x,0,0);
            _transform.localRotation = Quaternion.Euler(0, lookRotation.y, 0);
        }

        private void UpdatePosition()
        {
            _transform.position = clientInputHandler.ServerPositionValue;
        }
    }
}