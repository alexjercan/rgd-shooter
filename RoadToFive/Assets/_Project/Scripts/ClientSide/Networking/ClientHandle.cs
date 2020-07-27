using System.Collections.Generic;
using System.Net;
using _Project.Scripts.DataStructure;
using UnityEngine;

namespace _Project.Scripts.ClientSide.Networking
{
    public static class ClientHandle
    {
        public delegate void PacketHandler(Packet packet);
        public static Dictionary<int, PacketHandler> PacketHandlers;

        public static void InitializeClientData()
        {
            PacketHandlers = new Dictionary<int, PacketHandler>()
            {
                {(int)ServerPackets.Welcome, Welcome},
                {(int)ServerPackets.SpawnPlayer, SpawnPlayer},
                {(int)ServerPackets.PlayerPosition, PlayerPosition},
                {(int)ServerPackets.PlayerRotation, PlayerRotation},
                {(int)ServerPackets.PlayerDisconnected, PlayerDisconnected},
            };
        }

        private static void Welcome(Packet packet)
        {
            var message = packet.ReadString();
            var myId = packet.ReadInt();
            
            Debug.Log($"Welcome received {message}");
            
            Client.MyId = myId;
            ClientSend.WelcomeReceived();
            
            Client.Connection.Udp.Connect(((IPEndPoint)Client.Connection.Tcp.Socket.Client.LocalEndPoint).Port);
        }

        private static void SpawnPlayer(Packet packet)
        {
            var id = packet.ReadInt();
            var username = packet.ReadString();
            var position = packet.ReadVector3();
            var rotation = packet.ReadQuaternion();
            
            GameManager.Instance.SpawnPlayer(id, username, position, rotation);
        }

        private static void PlayerPosition(Packet packet)
        {
            var id = packet.ReadInt();
            var position = packet.ReadVector3();

            GameManager.Instance.playerManagers[id].PlayerTransform.position = position;
        }

        private static void PlayerRotation(Packet packet)
        {
            var id = packet.ReadInt();
            var rotation = packet.ReadQuaternion();

            GameManager.Instance.playerManagers[id].PlayerTransform.rotation = rotation;
        }

        private static void PlayerDisconnected(Packet packet)
        {
            var id = packet.ReadInt();

            GameManager.Instance.DeSpawn(id);
        }
    }
}