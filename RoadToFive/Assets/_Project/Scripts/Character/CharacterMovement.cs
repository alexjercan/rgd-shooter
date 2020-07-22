using System;
using UnityEngine;

namespace _Project.Scripts.Character
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
        
        public Vector3 GetControllerInput(Vector2 movementInput, bool jumpInput, Vector3 forward, Vector3 right,
            bool isGrounded, float jumpHeight, float movementSpeed)
        {
            var movementDirection = right * movementInput.x + forward * movementInput.y;

            _velocity.y = (float) (isGrounded
                ? (jumpInput ? Math.Sqrt(-2 * jumpHeight * _gravity) : 0)
                : _velocity.y + _gravity * Time.deltaTime);

            var controllerInput = movementDirection * movementSpeed + _velocity;
            return controllerInput;
        }
    }
}