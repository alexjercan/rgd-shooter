using _Project.Scripts.Mechanics;
using _Project.Scripts.ServerSide.Networking;
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

        [SerializeField] public PlayerMovement playerMovement;
        [SerializeField] public PlayerWeapon playerWeapon;
        [SerializeField] public EntityHealth entityHealth;
        [SerializeField] public PlayerPickUp playerPickUp;
        [SerializeField] public PlayerInventory playerInventory;
        

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
    }
}