using System;
using System.Net;
using System.Net.Sockets;

namespace _Project.Scripts.Networking.UDP
{
    public class UserDatagramProtocolClient : IUserDatagramProtocolSocket
    {
        private readonly UdpClient _socket;

        /// <summary>
        ///Initializes a new instance of the UdpClient class and binds it to the specified local endpoint.
        ///It also binds the UDP Socket to the default remote host.
        /// </summary>
        /// <param name="localEndPoint"></param>
        /// <param name="defaultRemoteHost"></param>
        public UserDatagramProtocolClient(IPEndPoint localEndPoint, IPEndPoint defaultRemoteHost)
        {
            _socket = new UdpClient(localEndPoint);
            _socket.Connect(defaultRemoteHost);
        }
        
        /// <summary>
        /// Starts listening asynchronously for a datagram incoming on the socket.
        /// When a datagram is received a callback is invoked.
        /// </summary>
        /// <param name="requestCallback"></param>
        public void Listen(AsyncCallback requestCallback)
        {
            _socket.BeginReceive(requestCallback, _socket);
        }

        /// <summary>
        /// Sends a datagram to the default remote host.
        /// </summary>
        /// /// <param name="datagram"></param>
        /// <param name="remoteHost"></param>
        public void SendDatagram(byte[] datagram, IPEndPoint remoteHost)
        {
            _socket.BeginSend(datagram, datagram.Length, null, null);
        }
    }
}








