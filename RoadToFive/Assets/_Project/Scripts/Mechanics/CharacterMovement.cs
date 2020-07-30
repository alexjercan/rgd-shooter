using System;
using UnityEngine;

namespace _Project.Scripts.ServerSide.Movement
{
    public class CharacterMovement
    {
        private const float GravityAcceleration = -9.81f * 2;

        private Vector3 _velocity;
        private float _currentTurnVelocity;

        public Vector3 GetControllerInput(Vector3 movementInput, Vector3 forward, Vector3 right,
            bool isGrounded, float jumpHeight, float movementSpeed)
        {
            var movementDirection = right * movementInput.x + forward * movementInput.z;
            var jumpInput = movementInput.y;

            _velocity.y = (float) (isGrounded
                ? jumpInput > 0.0f ? Math.Sqrt(-2 * jumpHeight * GravityAcceleration) : 0
                : _velocity.y + GravityAcceleration * Time.fixedDeltaTime);

            var controllerInput = movementDirection * movementSpeed + _velocity;
            return controllerInput;
        }
    }
}