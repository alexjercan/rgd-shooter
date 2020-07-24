namespace _Project.Scripts.Networking
{
    public interface IServerManager
    {
        void SendMessage(int clientId, byte[] message);
    }
}