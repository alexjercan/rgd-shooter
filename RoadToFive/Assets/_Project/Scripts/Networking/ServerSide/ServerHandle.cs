using UnityEngine;

namespace _Project.Scripts.Networking.ServerSide
{
    public static class ServerHandle
    {
        public static void WelcomeReceived(int fromClient, Packet packet)
        {
            var clientIdCheck = packet.ReadInt();
            var username = packet.ReadString();
            
            if (clientIdCheck != fromClient) return;
            
            Server.ClientConnections[fromClient].SendIntoGame(username);
        }
    }
}