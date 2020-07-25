using System;
using UnityEngine;

namespace _Project.Scripts.Movement.Mechanics
{
    public class CharacterMovement
    {
        private readonly float _gravity;

        private Vector3 _velocity;
        private float _currentTurnVelocity;

        public CharacterMovement(float gravity)
        {
            _gravity = gravity;
        }
        
        public Vector3 GetControllerInput(Vector3 movementInput, Vector3 forward, Vector3 right,
            bool isGrounded, float jumpHeight, float movementSpeed)
        {
            var movementDirection = right * movementInput.x + forward * movementInput.z;
            var jumpInput = movementInput.y;

            _velocity.y = (float) (isGrounded
                ? jumpInput > 0.0f ? Math.Sqrt(-2 * jumpHeight * _gravity) : 0
                : _velocity.y + _gravity * Time.fixedDeltaTime);

            var controllerInput = movementDirection * movementSpeed + _velocity;
            return controllerInput;
        }
    }
}