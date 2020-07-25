using _Project.Scripts.Networking;
using UnityEngine;

namespace _Project.Scripts
{
    public class ServerPlayerManager : MonoBehaviour
    { 
        public PlayerData PlayerData { get; set; }
        public Vector3 PlayerMovementInput { get; set; }
        public Vector2 PlayerRotation { get; set; }
    }
}