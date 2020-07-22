using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts
{
    public class ClientInputHandler : MonoBehaviour
    {
        [SerializeField] private ClientCharacterController clientCharacterController;
        

        public void MovementInputCallback(InputAction.CallbackContext context) =>
            clientCharacterController.MovementInput = context.ReadValue<Vector2>();

        public void JumpInputCallback(InputAction.CallbackContext context) =>
            clientCharacterController.JumpInput = context.ReadValueAsButton();

        public void LookInputCallback(InputAction.CallbackContext context) =>
            clientCharacterController.LookInput = context.ReadValue<Vector2>();
    }
}