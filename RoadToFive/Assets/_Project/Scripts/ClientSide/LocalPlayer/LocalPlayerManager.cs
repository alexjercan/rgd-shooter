using _Project.Scripts.ClientSide.Networking;
using _Project.Scripts.ClientSide.Player;
using UnityEngine;

namespace _Project.Scripts.ClientSide.LocalPlayer
{
    /// <summary>
    /// PLAYER MANAGER CONTINE TOATE COMPONENTELE CARE DEFINESC JUCATORUL LOCAL
    /// </summary>
    public class LocalPlayerManager : MonoBehaviour
    {
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private PlayerMovementInput playerMovementInput;
        [SerializeField] private PlayerRotation playerRotation;
        [SerializeField] private PlayerShootInput playerShootInput;

        private void FixedUpdate()
        {
            ClientSend.PlayerMovement(playerMovementInput.GetMovementInput(),  playerRotation.GetRotation());
            
            if (playerShootInput.GetShootInput()) ClientSend.PlayerShoot(playerRotation.GetForwardDirection(), playerManager.GetHandWeaponId());

            var weaponIndex = playerShootInput.GetWeaponIndex(playerManager.GetWeaponCount() - 1);
            if (weaponIndex >= 0) ClientSend.HandWeapon(weaponIndex);
        }
    }
}