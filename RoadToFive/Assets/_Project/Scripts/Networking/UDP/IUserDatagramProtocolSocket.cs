using System;
using System.Net;
using System.Net.Sockets;

namespace _Project.Scripts.Networking.UDP
{
    public interface IUserDatagramProtocolSocket
    {
        UdpClient Socket { get; }
        IPEndPoint LocalIpEndPoint { get; }

        void Listen(AsyncCallback requestCallback);
        
        void SendDatagram(byte[] datagram, IPEndPoint remoteHost);
    }
}