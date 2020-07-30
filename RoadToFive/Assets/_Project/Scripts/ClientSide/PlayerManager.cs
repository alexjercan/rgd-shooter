using _Project.Scripts.Mechanics;
using UnityEngine;

namespace _Project.Scripts.ClientSide
{
    /// <summary>
    /// PLAYER MANAGER CONTINE TOATE COMPONENTELE CARE DEFINESC UN JUCATOR
    /// </summary>
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private EntityHealth entityHealth;

        private int _id;
        private string _username;

        private Transform _transform;

        private void Awake() => _transform = GetComponent<Transform>();

        public void Initialize(int id, string username)
        {
            _id = id;
            _username = username;
            entityHealth.Died += (sender, args) => gameObject.SetActive(false);
        }

        public void SetPosition(Vector3 position) => _transform.position = position;

        public void SetRotation(Quaternion rotation) => _transform.rotation = rotation;

        public void SetHealth(int health) => entityHealth.SetHealth(health);
    }
}
