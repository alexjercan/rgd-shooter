using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace _Project.Scripts.Networking.ClientSide
{
    public class ClientHandle
    {
        public delegate void PacketHandler(Packet packet);
        public static Dictionary<int, PacketHandler> PacketHandlers;

        public static void InitializeClientData()
        {
            PacketHandlers = new Dictionary<int, PacketHandler>()
            {
                {(int)ServerPackets.Welcome, ClientHandle.Welcome},
                {(int)ServerPackets.SpawnPlayer, ClientHandle.SpawnPlayer},
                {(int)ServerPackets.PlayerPosition, ClientHandle.PlayerPosition},
                {(int)ServerPackets.PlayerRotation, ClientHandle.PlayerRotation},
                {(int)ServerPackets.PlayerDisconnected, ClientHandle.PlayerDisconnected},
            };
        }
        
        public static void Welcome(Packet packet)
        {
            var message = packet.ReadString();
            var myId = packet.ReadInt();
            
            Debug.Log($"Welcome received {message}");
            
            Client.MyId = myId;
            ClientSend.WelcomeReceived();
            
            Client.Connection.Udp.Connect(((IPEndPoint)Client.Connection.Tcp.Socket.Client.LocalEndPoint).Port);
        }

        public static void SpawnPlayer(Packet packet)
        {
            var id = packet.ReadInt();
            var username = packet.ReadString();
            var position = packet.ReadVector3();
            var rotation = packet.ReadQuaternion();
            
            GameManager.Instance.SpawnPlayer(id, username, position, rotation);
        }

        public static void PlayerPosition(Packet packet)
        {
            var id = packet.ReadInt();
            var position = packet.ReadVector3();

            GameManager.Instance.playerManagers[id].PlayerTransform.position = position;
        }

        public static void PlayerRotation(Packet packet)
        {
            var id = packet.ReadInt();
            var rotation = packet.ReadQuaternion();

            GameManager.Instance.playerManagers[id].PlayerTransform.rotation = rotation;
        }

        public static void PlayerDisconnected(Packet packet)
        {
            var id = packet.ReadInt();

            GameManager.Instance.DeSpawn(id);
        }
    }
}