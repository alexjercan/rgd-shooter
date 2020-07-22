using UnityEngine;

namespace _Project.Scripts
{
    public class ClientCharacterController : MonoBehaviour
    {
        public Vector2 MovementInput { get; set; }
        public Vector2 LookInput { get; set; }
        public bool JumpInput { get; set; }
        public Vector3 ServerPositionValue { get; set; }
        public Quaternion ClientRotationValue { get; set; }
    }
}