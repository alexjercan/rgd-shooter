using System.Net;
using UnityEngine;

namespace _Project.Scripts.Networking.ClientSide
{
    public class ClientHandle
    {
        public static void Welcome(Packet packet)
        {
            var message = packet.ReadString();
            var myId = packet.ReadInt();
            
            Debug.Log($"Welcome received {message}");
            
            Client.MyId = myId;
            ClientSend.WelcomeReceived();
            
            Client.Socket.UdpConnect(((IPEndPoint)Client.Socket.TcpSocket.Client.LocalEndPoint).Port);
        }
    }
}