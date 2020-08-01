using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.ClientSide.LocalPlayer
{
    public class PlayerShootInput : MonoBehaviour
    {
        private bool _shootInput;
        private int _weaponIndex = 0;
        private bool _weaponChanged;

        public void ShootInputCallback(InputAction.CallbackContext context) => 
            _shootInput = context.ReadValueAsButton();

        public void ScrollInputCallback(InputAction.CallbackContext context)
        {
            _weaponChanged = true;
            var value = (int) context.ReadValue<float>();
            _weaponIndex += value;
        }

        public bool GetShootInput()
        {
            var value = _shootInput;
            _shootInput = false;
            return value;
        }

        public bool DidWeaponChanged()
        {
            var value = _weaponChanged;
            _weaponChanged = false;
            return value;
        }

        public int GetWeaponIndex(int weaponListSize)
        {
            if (weaponListSize <= 0) return -1;
            _weaponIndex = (_weaponIndex + weaponListSize) % weaponListSize;
            return _weaponIndex;
        }
    }
}