using _Project.Scripts.Movement.Mechanics;
using UnityEngine;

namespace _Project.Scripts.Networking.ServerSide
{
    public class PlayerManager : MonoBehaviour
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public Vector3 MovementInput { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        
        [SerializeField] private CharacterController characterController;
        [SerializeField] private float movementSpeed = 12.0f;
        [SerializeField] private float jumpHeight = 3.0f;

        private Transform _transform;
        private CharacterMovement _characterMovement;
        
        private void Awake() => _transform = GetComponent<Transform>();
        
        private void Start() => _characterMovement = new CharacterMovement();

        private void FixedUpdate()
        {
            _transform.localRotation = Rotation;
            
            MoveCharacter();
            
            ServerSend.PlayerPosition(this);
            ServerSend.PlayerRotation(this);
        }
        
        public void Initialize(int id, string username, Vector3 position, Quaternion rotation)
        {
            Id = id;
            Username = username;
            Position = position;
            Rotation = rotation;
        }

        private void MoveCharacter()
        {
            var controllerInput = _characterMovement.GetControllerInput(MovementInput, _transform.forward, _transform.right, characterController.isGrounded, jumpHeight, movementSpeed);
            characterController.Move(controllerInput * Time.fixedDeltaTime);

            Position = _transform.position;
        }
    }
}