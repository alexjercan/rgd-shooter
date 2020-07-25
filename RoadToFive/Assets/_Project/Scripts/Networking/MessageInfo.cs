namespace _Project.Scripts.Networking
{
    public enum MessageType : int
    {
        Invalid = 0,
        Dummy,
        DummyAck,
        Welcome,
        WelcomeAck,
        SpawnPlayer,
        PlayerInput,
        PlayerDisconnect,
        ServerDisconnect,
    }
}