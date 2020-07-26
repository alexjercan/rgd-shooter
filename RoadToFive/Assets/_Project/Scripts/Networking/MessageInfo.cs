namespace _Project.Scripts.Networking
{
    public enum MessageType : int
    {
        Invalid = 0,
        Welcome,
        WelcomeAck,
        SpawnPlayer,
        PlayerInput,
        PlayerMovement,
        PlayerDisconnect,
        ServerDisconnect,
    }
}