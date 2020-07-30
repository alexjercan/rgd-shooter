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

        public Transform PlayerTransform { get; private set; }

        private void Awake() => PlayerTransform = GetComponent<Transform>();
    }
}