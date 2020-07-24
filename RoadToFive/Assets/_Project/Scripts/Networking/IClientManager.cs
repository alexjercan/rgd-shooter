namespace _Project.Scripts.Networking
{
    public interface IClientManager
    {
        void SendMessage(int senderId, byte[] message);
    }
}