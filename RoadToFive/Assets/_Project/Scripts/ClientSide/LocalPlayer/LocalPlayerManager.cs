﻿using _Project.Scripts.ClientSide.Networking;
using _Project.Scripts.ClientSide.Player;
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

        private void Start()
        {
            playerManager.entityHealth.Died += (sender, args) =>
            {
                Debug.Log("YOU DIED LOL NOOB");
                playerRotation.playerCamera.transform.SetParent(null);
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