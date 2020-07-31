using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.ClientSide.LocalPlayer
{
    /// <summary>
    /// COMPONENTA SPECIFICA LOCAL-PLAYER-ULUI CARE SE OCUPA DE MOVEMENT INPUT
    /// </summary>
    public class PlayerMovementInput : MonoBehaviour //DONE DO NOT MODIFY
    {
        private Vector2 _movementInput;
        private bool _jumpInput;

        public void MovementInputCallback(InputAction.CallbackContext context) => 
            _movementInput = context.ReadValue<Vector2>();

        public void JumpInputCallback(InputAction.CallbackContext context) => 
            _jumpInput = context.ReadValueAsButton();

        public Vector3 GetMovementInput() => new Vector3(_movementInput.x, _jumpInput ? 1 : 0, _movementInput.y);
    }
}