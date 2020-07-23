using System.Net;
using System.Net.Sockets;

namespace _Project.Scripts.Networking.UDP
{
    public abstract class UdpBinding
    {
        public UdpClient Socket { get; protected set; }
        public abstract IPEndPoint EndPoint { get; }
        
        public abstract void Bind(IPEndPoint endPoint);
    }
}