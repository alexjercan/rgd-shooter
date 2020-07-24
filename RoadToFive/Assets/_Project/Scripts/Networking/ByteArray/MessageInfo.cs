namespace _Project.Scripts.Networking.ByteArray
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
    }
}