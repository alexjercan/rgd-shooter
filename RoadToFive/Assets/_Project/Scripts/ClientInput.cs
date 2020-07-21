using _Project.Scripts.Packet;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts
{
    public class ClientInput : MonoBehaviour
    {
        [SerializeField] private NetworkManagerMock networkManager;
        [SerializeField] private ClientCharacterController clientCharacterController;

        public void MovementInputCallback(InputAction.CallbackContext context)
        {
            var packetBuilder = new PacketBuilder((int) ClientPacket.MovementInput);
            var data = context.ReadValue<Vector2>();
            var bytes = packetBuilder.Write(data).ToByteArray();
            networkManager.SendPacketToServer(bytes);
        }

        public void JumpInputCallback(InputAction.CallbackContext context)
        {
            var packetBuilder = new PacketBuilder((int) ClientPacket.JumpInput);
            var data = context.ReadValueAsButton();
            var bytes = packetBuilder.Write(data).ToByteArray();
            networkManager.SendPacketToServer(bytes);
        }

        public void ShootInputCallback(InputAction.CallbackContext context)
        {
            var packetBuilder = new PacketBuilder((int) ClientPacket.ShootInput);
            var data = context.ReadValueAsButton();
            var bytes = packetBuilder.Write(data).ToByteArray();
            networkManager.SendPacketToServer(bytes);
        }
        
        public void LookInputCallback(InputAction.CallbackContext context)
        {
            clientCharacterController.RotationInput = context.ReadValue<Vector2>();
            var data = clientCharacterController.transform.rotation;
            var packetBuilder = new PacketBuilder((int) ClientPacket.RotationPacket);
            var bytes = packetBuilder.Write(data).ToByteArray();
            networkManager.SendPacketToServer(bytes);
        }
    }
}