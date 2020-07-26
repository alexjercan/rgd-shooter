using _Project.Scripts.ByteArray;
using _Project.Scripts.Movement.Character;
using _Project.Scripts.Networking;
using UnityEngine;

namespace _Project.Scripts.SynchronizationComponents
{
    internal class NetworkMovementInput : MonoBehaviour, INetworkTransferable
    {
        [SerializeField] private LocalCharacterController localCharacterController;
        
        public MessageType Type => MessageType.PlayerInput;
        
        public byte[] Serialize()
        {
            var movementInput = new Vector3(localCharacterController.MovementInput.x, localCharacterController.JumpInput ? 1 : 0, localCharacterController.MovementInput.y);
            var rotation = localCharacterController.PlayerRotation;
            return new ByteArrayBuilder().Write(movementInput).Write(rotation).ToByteArray();
        }
    }
}