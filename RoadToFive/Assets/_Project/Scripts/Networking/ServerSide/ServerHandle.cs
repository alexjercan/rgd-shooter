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
            
            Debug.Log($"Spawning player {username}");
            ServerManager.Instance.SendIntoGame(fromClient, username);
        }

        public static void PlayerMovement(int fromClient, Packet packet)
        {
            var movementInput = packet.ReadVector3();
            var rotation = packet.ReadQuaternion();

            ServerManager.Instance.playerManagers[fromClient].SetInput(movementInput, rotation);
        }
    }
}