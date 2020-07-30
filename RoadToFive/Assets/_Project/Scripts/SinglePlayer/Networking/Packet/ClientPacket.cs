namespace _Project.Scripts.Networking.Packet
{
    public enum ClientPacket : int
    {
        InvalidPacket = 0,
        MovementInput,
        JumpInput,
        ShootInput,
        RotationPacket,
    }
}