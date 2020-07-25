using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Movement
{
    public class ClientInputHandler : MonoBehaviour
    {
        public Vector2 MovementInput { get; set; }
        public Vector2 LookInput { get; set; }
        public bool JumpInput { get; set; }

        public void MovementInputCallback(InputAction.CallbackContext context) => MovementInput = context.ReadValue<Vector2>();

        public void JumpInputCallback(InputAction.CallbackContext context) => JumpInput = context.ReadValueAsButton();

        public void LookInputCallback(InputAction.CallbackContext context) => LookInput = context.ReadValue<Vector2>();
    }
}