using _Project.Scripts.Networking;

namespace _Project.Scripts
{
    public interface INetworkTransferable
    {
        MessageType Type { get; }
        byte[] Serialize();
    }
}