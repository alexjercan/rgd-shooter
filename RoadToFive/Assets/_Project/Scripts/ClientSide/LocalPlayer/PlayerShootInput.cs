using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.ClientSide.LocalPlayer
{
    public class PlayerShootInput : MonoBehaviour
    {
        private bool _shootInput;
        
        public void JumpInputCallback(InputAction.CallbackContext context) => 
            _shootInput = context.ReadValueAsButton();

        public bool GetShootInput() => _shootInput;
    }
}