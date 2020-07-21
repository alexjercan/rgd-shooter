using System;
using UnityEngine;

namespace _Project.Scripts
{
    public class NetworkManagerMock : MonoBehaviour
    {
        [SerializeField] private ServerCharacterController serverCharacterController;
        [SerializeField] private Transform clientTransform;
        
        private Transform _serverCharacterControllerTransform;

        private void Awake()
        {
            _serverCharacterControllerTransform = serverCharacterController.transform;
        }

        public void SendPacketToServer(byte[] bytes)
        {
            var packetReader = new PacketReader(bytes);
            var packetType = (ClientPacket)packetReader.ReadInt();

            switch (packetType)
            {
                case ClientPacket.InvalidPacket:
                    break;
                case ClientPacket.MovementInput:
                    serverCharacterController.MovementInput = packetReader.ReadVector2();
                    break;
                case ClientPacket.JumpInput:
                    serverCharacterController.JumpInput = packetReader.ReadBool();
                    break;
                case ClientPacket.ShootInput:
                    break;
                case ClientPacket.CameraYInput:
                    serverCharacterController.CameraYRotation = packetReader.ReadFloat();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void SendPacketToClient(byte[] bytes)
        {
            var packetReader = new PacketReader(bytes);
            var packetType = (ServerPacket)packetReader.ReadInt();

            switch (packetType)
            {
                case ServerPacket.InvalidPacked:
                    break;
                case ServerPacket.PositionPacket:
                    clientTransform.position = packetReader.ReadVector3();
                    break;
                case ServerPacket.RotationPacket:
                    clientTransform.rotation = packetReader.ReadQuaternion();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Update()
        {
            var position = _serverCharacterControllerTransform.position;
            
            var packetBuilder = new PacketBuilder((int) ServerPacket.PositionPacket);
            var bytes = packetBuilder.Write(position).ToByteArray();
            SendPacketToClient(bytes);

            var rotation = _serverCharacterControllerTransform.rotation;
            
            packetBuilder = new PacketBuilder((int) ServerPacket.RotationPacket);
            bytes = packetBuilder.Write(rotation).ToByteArray();
            SendPacketToClient(bytes);
        }
    }
}