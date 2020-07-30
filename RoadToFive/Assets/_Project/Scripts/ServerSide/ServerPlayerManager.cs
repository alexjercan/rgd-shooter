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
        
        private void FixedUpdate()
        {
            ServerSend.PlayerPosition(Id, playerMovement);
            ServerSend.PlayerRotation(Id, playerMovement);
        }
        
        public void Initialize(int id, string username)
        {
            Id = id;
            Username = username;
        }

        public void SetInput(Vector3 movementInput, Quaternion rotation) => playerMovement.SetInput(movementInput, rotation);
        
        public Vector3 GetPlayerPosition() => playerMovement.PlayerTransform.position;

        public Quaternion GetPlayerRotation() => playerMovement.PlayerTransform.rotation;
    }
}