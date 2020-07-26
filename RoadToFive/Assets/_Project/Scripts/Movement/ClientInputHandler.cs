using _Project.Scripts.Movement.Character;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Movement
{
    public class ClientInputHandler : MonoBehaviour
    {
        [SerializeField] private LocalCharacterController localCharacterController;
        
        public void MovementInputCallback(InputAction.CallbackContext context) => localCharacterController.MovementInput = context.ReadValue<Vector2>();

        public void JumpInputCallback(InputAction.CallbackContext context) => localCharacterController.JumpInput = context.ReadValueAsButton();

        public void LookInputCallback(InputAction.CallbackContext context) => localCharacterController.LookInput = context.ReadValue<Vector2>();
    }
}