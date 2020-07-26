using System;
using System.Net;
using System.Net.Sockets;

namespace _Project.Scripts.Networking.UDP
{
    public class ClientSocket
    {
        private UdpClient _socket;
        private IPEndPoint _remoteHostEndPoint;

        /// <summary>
        /// Initializes a new instance of the ClientSocket class and binds it to the specified local endpoint.
        /// It also binds the UDP Socket to the default remote host. This represents the socket used by the
        /// client to communicate with the socket on the remote host.
        /// </summary>
        /// <param name="localEndPoint"></param>
        /// <param name="defaultRemoteHost"></param>
        public ClientSocket(IPEndPoint localEndPoint, IPEndPoint defaultRemoteHost)
        {
            _remoteHostEndPoint = default;
            _socket = new UdpClient(localEndPoint);
            _socket.Connect(defaultRemoteHost);
        }
        
        /// <summary>
        /// Starts listening asynchronously for a datagram incoming on the socket.
        /// When a datagram is received a callback is invoked.
        /// </summary>
        /// <param name="requestCallback"></param>
        public void Listen(AsyncCallback requestCallback) => _socket.BeginReceive(requestCallback, null);

        public byte[] EndReceive(IAsyncResult asyncResult) => _socket.EndReceive(asyncResult, ref _remoteHostEndPoint);

        /// <summary>
        /// Sends a datagram to the default remote host.
        /// </summary>
        /// <param name="datagram"></param>
        public void SendDatagram(byte[] datagram) => _socket.BeginSend(datagram, datagram.Length, null, null);

        public void Disconnect()
        {
            if (_socket == null) return;
            
            _socket.Close();
            _socket = null;
        }
    }
}








