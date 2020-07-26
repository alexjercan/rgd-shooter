﻿using System;
using System.Net;
using System.Net.Sockets;

namespace _Project.Scripts.Networking.UDP
{
    public class ServerSocket
    {
        private readonly UdpClient _socket;

        /// <summary>
        /// Initializes a new instance of the UdpClient class and binds it to the specified local endpoint.
        /// This represents the socket used by the server to communicate with the clients.
        /// </summary>
        /// <param name="port"></param>
        public ServerSocket(int port) => _socket = new UdpClient(port, AddressFamily.InterNetwork);

        /// <summary>
        /// Starts listening asynchronously for a datagram incoming on the socket.
        /// When a datagram is received a callback is invoked.
        /// </summary>
        /// <param name="requestCallback"></param>
        public void Listen(AsyncCallback requestCallback) => _socket.BeginReceive(requestCallback, null);

        public byte[] EndReceive(IAsyncResult asyncResult, ref IPEndPoint remoteHostEndPoint) => _socket.EndReceive(asyncResult, ref remoteHostEndPoint);

        
        /// <summary>
        /// Sends a datagram to the specified host.
        /// </summary>
        /// <param name="datagram"></param>
        /// <param name="remoteHost"></param>
        public void SendDatagram(byte[] datagram, IPEndPoint remoteHost) => _socket.BeginSend(datagram, datagram.Length, remoteHost, null, null);

        public void Disconnect()
        {
            _socket.Close();
        }
    }
}