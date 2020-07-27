﻿using _Project.Scripts.Movement.Mechanics;
using UnityEngine;

namespace _Project.Scripts.Networking.ServerSide
{
    public class PlayerManager : MonoBehaviour
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public Vector3 MovementInput { get; set; }
        public Transform PlayerTransform { get; private set; }

        [SerializeField] private CharacterController characterController;
        [SerializeField] private float movementSpeed = 6.0f;
        [SerializeField] private float jumpHeight = 1.0f;

        private CharacterMovement _characterMovement;
        
        private void Awake() => PlayerTransform = GetComponent<Transform>();
        
        private void Start() => _characterMovement = new CharacterMovement();

        private void FixedUpdate()
        {
            MoveCharacter();
            
            ServerSend.PlayerPosition(this);
            ServerSend.PlayerRotation(this);
        }
        
        public void Initialize(int id, string username)
        {
            Id = id;
            Username = username;
        }

        public void SetInput(Vector3 movementInput, Quaternion rotation)
        {
            MovementInput = movementInput;
            PlayerTransform.rotation = rotation;
        }

        private void MoveCharacter()
        {
            var controllerInput = _characterMovement.GetControllerInput(MovementInput, PlayerTransform.forward, PlayerTransform.right, characterController.isGrounded, jumpHeight, movementSpeed);
            characterController.Move(controllerInput * Time.fixedDeltaTime);
        }
    }
}