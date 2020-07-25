using _Project.Scripts.Networking;
using UnityEngine;
using Quaternion = System.Numerics.Quaternion;
using Vector3 = System.Numerics.Vector3;

namespace _Project.Scripts
{
    public class PlayerManager : MonoBehaviour
    {
        public PlayerData PlayerData { get; set; }

        public void Move(Vector3 input)
        {
            Debug.Log("MOVING");
        }

        public void Rotate(Quaternion rotation)
        {
            Debug.Log("ROTATING");
        }
    }
}