using System;
using _Project.Scripts.ClientSide.Networking;
using _Project.Scripts.ClientSide.Player;
using _Project.Scripts.ClientSide.UserInterface;
using UnityEngine;

namespace _Project.Scripts.ClientSide.LocalPlayer
{
    /// <summary>
    /// PLAYER MANAGER CONTINE TOATE COMPONENTELE CARE DEFINESC JUCATORUL LOCAL
    /// </summary>
    public class LocalPlayerManager : MonoBehaviour
    {
        public PlayerManager playerManager;
        public PlayerMovementInput playerMovementInput;
        public PlayerRotation playerRotation;
        public PlayerShootInput playerShootInput;
        public UiHealth uiHealth;
        public UiAmmo uiAmmo;
        
        private Camera _orbitCamera;

        private void Awake()
        {
            _orbitCamera = Camera.main;
        }

        private void Start()
        {
            _orbitCamera.gameObject.SetActive(false);
            
            playerManager.entityHealth.Died += (sender, args) =>
            {
                Debug.Log("YOU DIED LOL NOOB");
                _orbitCamera.gameObject.SetActive(true);
            };

            playerManager.entityHealth.Damaged += (sender, health) =>
            {
                uiHealth.DamageReceive(health.Health, health.MaxHealth);
            };

            playerManager.entityHealth.Healed += (sender, health) =>
            {
                uiHealth.HealReceive(health.Health, health.MaxHealth);
            };

            playerManager.playerInventory.AmmoChanged += (sender, inventory) =>
            {
                uiAmmo.AmmoIndicatorUpdate(inventory.Ammo);
            };
        }
        
        private void FixedUpdate()
        {
            ClientSend.PlayerMovement(playerMovementInput.GetMovementInput(),  playerRotation.GetRotation());
            
            if (playerShootInput.GetShootInput() && playerManager.handWeapon.MainWeaponId != -1) 
                ClientSend.PlayerShoot(playerRotation.GetForwardDirection(), playerManager.handWeapon.MainWeaponId);

            if (playerShootInput.DidWeaponChanged())
            {
                var weaponIndex = playerShootInput.GetWeaponIndex(playerManager.playerInventory.GetWeaponCount());
                if (weaponIndex >= 0) ClientSend.HandWeapon(weaponIndex);
            }
        }
    }
}