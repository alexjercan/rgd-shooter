using _Project.Scripts.Mechanics;
using _Project.Scripts.ServerSide.Networking;
using _Project.Scripts.ServerSide.Player;
using UnityEngine;

namespace _Project.Scripts.ServerSide
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
        [SerializeField] private PlayerInventory playerInventory;
        [SerializeField] private EntityHealth entityHealth;

        private void Awake()
        {
            entityHealth.Damaged += (sender, health) => ServerSend.PlayerHealth(Id, health);
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

        public void SetShootDirection(Vector3 direction) => playerWeapon.OnShoot(direction);
        
        public Vector3 GetPlayerPosition() => playerMovement.PlayerTransform.position;

        public Quaternion GetPlayerRotation() => playerMovement.PlayerTransform.rotation;
    }
}