using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Networking.ServerSide
{
    public static class ServerHandle
    {
        public delegate void PacketHandler(int fromClient, Packet packet);
        public static Dictionary<int, PacketHandler> PacketHandlers;
        
        public static void InitializeServerData()
        {
            PacketHandlers = new Dictionary<int, PacketHandler>()
            {
                {(int) ClientPackets.WelcomeReceived, ServerHandle.WelcomeReceived},
                {(int) ClientPackets.PlayerMovement, ServerHandle.PlayerMovement},
            };
        }
        
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