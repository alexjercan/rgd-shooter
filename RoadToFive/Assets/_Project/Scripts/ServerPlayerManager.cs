using UnityEngine;

namespace _Project.Scripts
{
    public class ServerPlayerManager : MonoBehaviour
    {
        public int ClientId { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 MovementInput { get; set; }
        public Vector2 Rotation { get; set; }
    }
}