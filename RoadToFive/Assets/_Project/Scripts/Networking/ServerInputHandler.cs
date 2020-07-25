using UnityEngine;

namespace _Project.Scripts.Networking
{
    public class ServerInputHandler : MonoBehaviour
    {
        public Vector2 MovementInput { get; set; }
        public bool JumpInput { get; set; }
        
        public float ClientRotationYValue { get; set; }
    }
}