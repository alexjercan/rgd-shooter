﻿using _Project.Scripts.Movement.Mechanics;
using UnityEngine;

namespace _Project.Scripts.Movement.Character
{
    public class LocalCharacterController : MonoBehaviour
    {
        public Vector2 MovementInput { get; set; }
        public Vector2 LookInput { get; set; }
        public bool JumpInput { get; set; }
        public Vector2 PlayerRotation { get; set; }
        
        [SerializeField] private ClientPlayerManager clientPlayerManager;
        [SerializeField] private Camera playerCamera;
        
        [Header("Camera settings")]
        [Range(0.1f, 5.0f)] [SerializeField] private float mouseSensitivity = 2.0f;
        [Range(0.0f, 180.0f)] [SerializeField] private float verticalRotationRange = 170.0f;
        [Range(1.0f, 5.0f)] [SerializeField] private float cameraSmoothing = 1.0f;

        private Transform _transform;
        private Transform _cameraTransform;
        private float _internalMouseSensitivity;

        private LookRotation _characterLookRotation;

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

        private void Update()
        {
            RotateCharacter();

            UpdateMovement();
        }

        private void RotateCharacter()
        {
            var mouseYInput = LookInput.y;
            var mouseXInput = LookInput.x;
            var lookRotation = _characterLookRotation.GetCharacterRotation(mouseXInput, mouseYInput,
                playerCamera.fieldOfView, _internalMouseSensitivity, verticalRotationRange, cameraSmoothing);
            _cameraTransform.localRotation = Quaternion.Euler(lookRotation.x,0,0);
            _transform.localRotation = Quaternion.Euler(0, lookRotation.y, 0);
            PlayerRotation = lookRotation;
        }
        
        private void UpdateMovement()
        {
            _transform.position = clientPlayerManager.PlayerPosition;
        }
    }
}