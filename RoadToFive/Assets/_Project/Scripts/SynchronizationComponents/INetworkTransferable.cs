using _Project.Scripts.Networking;

namespace _Project.Scripts.SynchronizationComponents
{
    public interface INetworkTransferable
    {
        MessageType Type { get; }
        byte[] Serialize();
    }
}