namespace _Project.Scripts.Networking
{
    public interface IServerManager
    {
        void SendMessage(int clientId, byte[] message);

        void BroadcastMessage(byte[] message);
        
        void BroadcastMessageExcept(int clientId, byte[] message);
    }
}