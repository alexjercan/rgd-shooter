using UnityEngine;

namespace _Project.Scripts
{
    public class ServerInputHandler : MonoBehaviour
    {
        public Vector2 MovementInput { get; set; }
        public bool JumpInput { get; set; }

        public Vector2 PlayerRotationValue { get; set; }
    }
}