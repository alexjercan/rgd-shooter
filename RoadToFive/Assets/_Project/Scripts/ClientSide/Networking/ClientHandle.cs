using System.Collections.Generic;
using System.Net;
using _Project.Scripts.Util.DataStructure;
using UnityEngine;

namespace _Project.Scripts.ClientSide.Networking
{
    /// <summary>
    /// SET DE FUNCTII CARE SERIALIZEAZA DATELE PRIMITE PRIN PACHETE
    /// </summary>
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
                {(int)ServerPackets.PlayerHealth, PlayerHealth},
                {(int)ServerPackets.CreateItemSpawner, CreateItemSpawner},
                {(int)ServerPackets.ItemSpawned, ItemSpawned},
                {(int)ServerPackets.ItemPickedUp, ItemPickedUp},
                {(int)ServerPackets.AmmoPickedUp, AmmoPickedUp},
                {(int)ServerPackets.WeaponPickedUp, WeaponPickedUp},
                {(int)ServerPackets.HandWeaponUpdate, HandWeaponUpdate},
                {(int)ServerPackets.InitializeInventory, InitializeInventory},
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

            GameManager.Instance.GetPlayerManager(id).transform.position = position;
        }

        private static void PlayerRotation(Packet packet)
        {
            var id = packet.ReadInt();
            var rotation = packet.ReadQuaternion();

            GameManager.Instance.GetPlayerManager(id).transform.rotation = rotation;
        }

        private static void PlayerDisconnected(Packet packet)
        {
            var id = packet.ReadInt();

            GameManager.Instance.DeSpawn(id);
        }

        private static void PlayerHealth(Packet packet)
        {
            var id = packet.ReadInt();
            var health = packet.ReadInt();
            GameManager.Instance.GetPlayerManager(id).entityHealth.SetHealth(health);
        }

        private static void CreateItemSpawner(Packet packet)
        {
            var spawnerId = packet.ReadInt();
            var spawnerPosition = packet.ReadVector3();
            var hasItem = packet.ReadBool();
            var itemId = packet.ReadInt();
            
            GameManager.Instance.CreateItemSpawner(spawnerId, spawnerPosition, hasItem, itemId);
        }

        private static void ItemSpawned(Packet packet)
        {
            var spawnerId = packet.ReadInt();

            GameManager.Instance.SpawnItem(spawnerId);
        }

        private static void ItemPickedUp(Packet packet)
        {
            var spawnerId = packet.ReadInt();
            var clientId = packet.ReadInt();

            GameManager.Instance.DeleteItem(spawnerId);
        }

        private static void AmmoPickedUp(Packet packet)
        {
            var id = packet.ReadInt();
            var amount = packet.ReadInt();
            
            GameManager.Instance.GetPlayerManager(id).playerInventory.AddAmmo(amount);
        }
        
        private static void WeaponPickedUp(Packet packet)
        {
            var id = packet.ReadInt();
            var weaponId = packet.ReadInt();
            
            GameManager.Instance.GetPlayerManager(id).playerInventory.AddWeapon(weaponId);
        }

        private static void HandWeaponUpdate(Packet packet)
        {
            var clientId = packet.ReadInt();
            var weaponIndex = packet.ReadInt();

            if (weaponIndex < 0) return;

            var playerManager = GameManager.Instance.GetPlayerManager(clientId);
            playerManager.handWeapon.SetWeaponTo(playerManager.playerInventory.GetWeaponAtIndex(weaponIndex));
        }

        private static void InitializeInventory(Packet packet)
        {
            var clientId = packet.ReadInt();
            var weaponCount = packet.ReadInt();
            var playerManager = GameManager.Instance.GetPlayerManager(clientId);
            for (var i = 0; i < weaponCount; i++) playerManager.playerInventory.AddWeapon(packet.ReadInt());
        }
    }
}
