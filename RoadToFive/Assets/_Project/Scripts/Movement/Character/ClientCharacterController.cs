using _Project.Scripts.Core;
using UnityEngine;

namespace _Project.Scripts.Movement.Character
{
    public class ClientCharacterController : MonoBehaviour
    {
        [SerializeField] private NetworkTransform networkTransform;
        
        private Transform _transform;
        
        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }
        
        private void Update()
        {
            RotateCharacter();

            UpdateMovement();
        }
        
        private void RotateCharacter()
        {
            _transform.localRotation = Quaternion.Euler(0, networkTransform.PlayerRotation.y, 0);
        }
        
        private void UpdateMovement()
        {
            _transform.position = networkTransform.PlayerPosition;
        }
    }
}