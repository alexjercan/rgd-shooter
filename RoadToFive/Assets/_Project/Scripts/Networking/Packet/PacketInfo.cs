namespace _Project.Scripts.Networking.Packet
{
    public static class PacketInfo
    {
        public const char NullTerminator = '\0';
    }
    
    public enum ClientPacket : int
    {
        InvalidPacket = 0,
    }
    
    public enum ServerPacket : int
    {
        InvalidPacket = 0,
        WelcomePacket,
    }
}