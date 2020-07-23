namespace _Project.Scripts.Networking.ByteArray
{
    public enum ClientPacket : int
    {
        InvalidPacket = 0,
        WelcomeReceived,
    }
    
    public enum ServerPacket : int
    {
        InvalidPacket = 0,
        WelcomePacket,
    }

    public enum ClientDatagram
    {
        InvalidDatagram = 0,
        WelcomeReceived,
    }

    public enum ServerDatagram
    {
        InvalidDatagram = 0,
        WelcomeDatagram,
    }
}