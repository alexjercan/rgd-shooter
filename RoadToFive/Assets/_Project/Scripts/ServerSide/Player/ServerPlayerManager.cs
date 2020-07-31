using System.Collections.Generic;
using _Project.Scripts.Mechanics;
using _Project.Scripts.ServerSide.Networking;
using _Project.Scripts.Util.Item;
using UnityEngine;

namespace _Project.Scripts.ServerSide.Player
{
    /// <summary>
    /// SERVER PLAYER MANAGER CONTINE TOATE COMPONENTELE CARE DEFINESC UN JUCATOR DPDV AL SERVERULUI
    /// </summary>
    public class ServerPlayerManager : MonoBehaviour
    {
        public int Id { get; private set; }
        public string Username { get; private set; }

        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private PlayerWeapon playerWeapon;
        [SerializeField] private EntityHealth entityHealth;
        [SerializeField] private PlayerPickUp playerPickUp;
        [SerializeField] private PlayerInventory playerInventory;
        

        private void Awake()
        {
            entityHealth.HealthChanged += (sender, health) => ServerSend.PlayerHealth(Id, health);
            entityHealth.Died += (sender, args) => gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            if (entityHealth.Health < 0) return;
            
            ServerSend.PlayerPosition(Id, playerMovement);
            ServerSend.PlayerRotation(Id, playerMovement);
        }
        
        public void Initialize(int id, string username)
        {
            Id = id;
            Username = username;
        }

        public void SetMovementInput(Vector3 movementInput, Quaternion rotation) => playerMovement.SetInput(movementInput, rotation);

        public void ShootInDirection(Vector3 direction, int weaponId) => playerWeapon.OnShoot(direction, weaponId);
        
        public Vector3 GetPlayerPosition() => playerMovement.PlayerTransform.position;

        public Quaternion GetPlayerRotation() => playerMovement.PlayerTransform.rotation;
        
        public void PickUpItem(ItemScriptableObject itemData) => playerPickUp.PickUpItem(this, itemData);
        
        public void HealPlayer(int amount) => entityHealth.Heal(amount);

        public void AddAmmo(int amount) => ServerSend.AmmoPickedUp(Id, amount);

        public void Weapon(int weaponId) => ServerSend.WeaponPickedUp(Id, weaponId);

        public List<int> GetWeapons() => playerInventory.GetWeapons();
    }
}