namespace _Project.Scripts
{
    public enum ClientPacket : int
    {
        InvalidPacket = 0,
        MovementInput,
        JumpInput,
        ShootInput,
        CameraYInput,
    }
}