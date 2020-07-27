using UnityEngine;

namespace _Project.Scripts.Networking.ClientSide
{
    public class PlayerManager : MonoBehaviour
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        
        private Transform _transform;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        private void FixedUpdate()
        {
            _transform.position = Position;
            _transform.rotation = Rotation;
        }
    }
}