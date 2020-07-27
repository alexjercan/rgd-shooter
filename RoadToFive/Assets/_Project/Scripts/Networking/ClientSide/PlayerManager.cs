using UnityEngine;

namespace _Project.Scripts.Networking.ClientSide
{
    public class PlayerManager : MonoBehaviour
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public Transform PlayerTransform { get; private set; }

        private void Awake() => PlayerTransform = GetComponent<Transform>();
    }
}