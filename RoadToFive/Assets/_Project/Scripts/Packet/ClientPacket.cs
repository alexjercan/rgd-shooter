namespace _Project.Scripts.Packet
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