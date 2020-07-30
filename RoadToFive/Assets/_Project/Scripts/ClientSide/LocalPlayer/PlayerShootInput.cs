using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.ClientSide.LocalPlayer
{
    public class PlayerShootInput : MonoBehaviour
    {
        private bool _shootInput;

        public void ShootInputCallback(InputAction.CallbackContext context) =>
            _shootInput = Mouse.current.leftButton.wasPressedThisFrame;

        public bool GetShootInput() => Mouse.current.leftButton.wasPressedThisFrame;
    }
}