using System;
using System.Net;

namespace _Project.Scripts.Networking.UDP
{
    public interface IUserDatagramProtocolSocket
    {
        void Listen(AsyncCallback requestCallback);
        
        void SendDatagram(byte[] datagram, IPEndPoint remoteHost);
    }
}