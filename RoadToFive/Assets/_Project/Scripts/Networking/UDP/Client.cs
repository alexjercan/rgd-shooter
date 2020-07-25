using System;
using System.Net;
using System.Net.Sockets;
using _Project.Scripts.Networking.ByteArray;

namespace _Project.Scripts.Networking.UDP
{
    public class Client
    {
        /// <summary>
        /// This event is being invoked when a message is received by the socket.
        /// </summary>
        public event EventHandler<ByteArrayReader> ReceivedDatagram;

        private readonly ClientSocket _socket;
        
        /// <summary>
        ///Initializes a new instance of the Client class and binds it to the specified local endpoint.
        ///It also binds the UDP Socket to the default remote host.
        /// </summary>
        /// <param name="remoteIp"></param>
        /// <param name="remotePort"></param>
        /// <param name="localPort"></param>
        public Client(string remoteIp, int remotePort, int localPort)
        {
            _socket = new ClientSocket(new IPEndPoint(IPAddress.Any, localPort),
                new IPEndPoint(IPAddress.Parse(remoteIp), remotePort));
        }

        /// <summary>
        /// Starts listening on the socket for datagrams from the remote host.
        /// </summary>
        public void Listen() => _socket.Listen(ReceiveCallback);

        /// <summary>
        /// Sends a datagram to the remote host. It also inserts an identifier at the beginning
        /// of the message.
        /// </summary>
        /// <param name="senderId"></param>
        /// <param name="datagram"></param>
        public void SendDatagram(int senderId, byte[] datagram)
        {
            var byteArrayBuilder = new ByteArrayBuilder().Write(senderId).Write(datagram).ToByteArray();
            _socket.SendDatagram(byteArrayBuilder);
        }
        
        public void Disconnect() => _socket.Disconnect();
        
        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            var socket = (UdpClient) asyncResult.AsyncState;
            var remoteHost = new IPEndPoint(IPAddress.Any, 0);
            var receivedBytes = socket.EndReceive(asyncResult, ref remoteHost);
            
            socket.BeginReceive(ReceiveCallback, socket);

            if (receivedBytes.Length <= 0)
            {
                _socket.Disconnect();
                return;
            }
            
            var receiveDatagram = new ByteArrayReader(receivedBytes);
            var datagramLength = receiveDatagram.ReadInt();
            if (datagramLength != receiveDatagram.UnreadBytes) return;

            OnReceivedDatagram(receiveDatagram);
        }

        private void OnReceivedDatagram(ByteArrayReader e) => ReceivedDatagram?.Invoke(this, e);
    }
}