using UnityEngine;

namespace _Project.Scripts.Networking.ServerSide
{
    public class ServerHandle
    {
        public static void WelcomeReceived(int fromClient, Packet packet)
        {
            var clientIdCheck = packet.ReadInt();
            var username = packet.ReadString();
            
            if (clientIdCheck != fromClient) return;
            
            Debug.Log("Spawning player");
            //TODO: SPAWN PLAYER
        }
    }
}