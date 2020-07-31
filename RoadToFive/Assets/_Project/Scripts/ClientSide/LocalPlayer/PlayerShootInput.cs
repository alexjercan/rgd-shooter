using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.ClientSide.LocalPlayer
{
    public class PlayerShootInput : MonoBehaviour
    {
        private bool _shootInput;

        public void ShootInputCallback(InputAction.CallbackContext context) => 
            _shootInput = context.ReadValueAsButton();

        public void ScrollInputCallback(InputAction.CallbackContext context)
        {
            //TODO: CHANGE WEAPON INDEX.
        }

        public bool GetShootInput()
        {
            var value = _shootInput;
            _shootInput = false;
            return value;
        }
    }
}