using _Project.Scripts.ClientSide.Networking;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.ClientSide.Movement
{
    /// <summary>
    /// COMPONENTA SPECIFICA LOCAL-PLAYER-ULUI CARE SE OCUPA DE MOVEMENT
    /// </summary>
    public class PlayerMovementInput : MonoBehaviour
    {
        private Transform _transform;
        
        private Vector2 _movementInput;
        private Vector2 _lookInput;
        private bool _jumpInput;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        private void FixedUpdate()
        {
            ClientSend.PlayerMovement(MovementInput(_movementInput, _jumpInput), _transform.rotation);
        }
        
        public void MovementInputCallback(InputAction.CallbackContext context) => 
            _movementInput = context.ReadValue<Vector2>();

        public void JumpInputCallback(InputAction.CallbackContext context) => 
            _jumpInput = context.ReadValueAsButton();

        private static Vector3 MovementInput(Vector2 movementInput, bool jumpInput) => 
            new Vector3(movementInput.x, jumpInput ? 1 : 0, movementInput.y);
    }
}