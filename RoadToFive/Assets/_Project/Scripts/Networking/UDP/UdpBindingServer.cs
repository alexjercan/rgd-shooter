using System.Net;

namespace _Project.Scripts.Networking.UDP
{
    class UdpBindingServer : UdpBinding
    {
        public override IPEndPoint EndPoint => _endPoint;
        
        private IPEndPoint _endPoint;

        public override void Bind(IPEndPoint endPoint)
        {
            _endPoint = endPoint;
        }
    }
}