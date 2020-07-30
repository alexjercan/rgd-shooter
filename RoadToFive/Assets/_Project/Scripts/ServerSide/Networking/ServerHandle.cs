using System.Collections.Generic;
using _Project.Scripts.DataStructure;
using UnityEngine;

namespace _Project.Scripts.ServerSide.Networking
{
    /// <summary>
    /// SET DE FUNCTII CARE SERIALIZEAZA DATELE PRIMITE PRIN PACHETE
    /// </summary>
    public static class ServerHandle
    {
        public delegate void PacketHandler(int fromClient, Packet packet);
        public static Dictionary<int, PacketHandler> PacketHandlers;
        
        public static void InitializeServerData()
        {
            PacketHandlers = new Dictionary<int, PacketHandler>()
            {
                {(int) ClientPackets.WelcomeReceived, WelcomeReceived},
                {(int) ClientPackets.PlayerMovement, PlayerMovement},
            };
        }

        private static void WelcomeReceived(int fromClient, Packet packet)
        {
            var clientIdCheck = packet.ReadInt();
            var username = packet.ReadString();
            
            if (clientIdCheck != fromClient) return;
            
            Debug.Log($"Spawning player {username}");
            ServerManager.Instance.SendIntoGame(fromClient, username);
        }

        private static void PlayerMovement(int fromClient, Packet packet)
        {
            var movementInput = packet.ReadVector3();
            var rotation = packet.ReadQuaternion();

            ServerManager.Instance.playerManagers[fromClient].SetInput(movementInput, rotation);
        }
    }
}