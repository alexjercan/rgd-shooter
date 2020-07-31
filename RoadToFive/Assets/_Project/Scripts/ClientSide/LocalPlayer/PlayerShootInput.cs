using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.ClientSide.LocalPlayer
{
    public class PlayerShootInput : MonoBehaviour
    {
        private bool _shootInput;
        private int _weaponIndex = 0;

        public void ShootInputCallback(InputAction.CallbackContext context) => 
            _shootInput = context.ReadValueAsButton();

        public void ScrollInputCallback(InputAction.CallbackContext context) => 
            _weaponIndex = context.ReadValue<int>();

        public bool GetShootInput()
        {
            var value = _shootInput;
            _shootInput = false;
            return value;
        }

        public int GetWeaponIndex(int maxWeaponIndex)
        {
            if (maxWeaponIndex < 0) return -1;
            if (maxWeaponIndex == 0) _weaponIndex = 0;
            else _weaponIndex %= maxWeaponIndex;
            return _weaponIndex;
        }
    }
}