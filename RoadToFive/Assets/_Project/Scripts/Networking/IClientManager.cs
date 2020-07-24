namespace _Project.Scripts.Networking
{
    public interface IClientManager
    {
        void SetClientId(int id);

        void SendMessage(byte[] message);
    }
}