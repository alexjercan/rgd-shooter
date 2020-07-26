﻿using _Project.Scripts.ByteArray;
 using _Project.Scripts.Core;
 using _Project.Scripts.Movement.Mechanics;
 using _Project.Scripts.Networking;
 using _Project.Scripts.SynchronizationComponents;
 using UnityEngine;

namespace _Project.Scripts.Movement.Character
{
    public class LocalCharacterController : MonoBehaviour, INetworkTransferable
    {
        public Vector2 MovementInput { get; set; }
        public Vector2 LookInput { get; set; }
        public bool JumpInput { get; set; }
        public Vector2 PlayerRotation { get; set; }
        
        [SerializeField] private NetworkTransform networkTransform;
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
        
        private void UpdateMovement() => _transform.position = networkTransform.PlayerPosition;

        public MessageType Type => MessageType.PlayerInput;
        
        public byte[] Serialize()
        {
            var movementInput = new Vector3(MovementInput.x, JumpInput ? 1 : 0, MovementInput.y);
            var rotation = PlayerRotation;
            return new ByteArrayBuilder().Write(movementInput).Write(rotation).ToByteArray();
        }
    }
}