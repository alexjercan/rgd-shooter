using UnityEngine;

namespace _Project.Scripts.ClientSide
{
    /// <summary>
    /// PLAYER MANAGER CONTINE TOATE COMPONENTELE CARE DEFINESC UN JUCATOR
    /// </summary>
    public class PlayerManager : MonoBehaviour
    {
        public int Id { get; set; }
        public string Username { get; set; }

        private Transform _transform;

        private void Awake() => _transform = GetComponent<Transform>();

        public void SetPosition(Vector3 position) => _transform.position = position;

        public void SetRotation(Quaternion rotation) => _transform.rotation = rotation;
    }
}