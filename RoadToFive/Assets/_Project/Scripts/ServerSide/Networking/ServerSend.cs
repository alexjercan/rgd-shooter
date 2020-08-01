using System.Collections.Generic;
using _Project.Scripts.Mechanics;
using _Project.Scripts.ServerSide.Enemy;
using _Project.Scripts.ServerSide.Player;
using _Project.Scripts.Util.DataStructure;
using UnityEngine;

namespace _Project.Scripts.ServerSide.Networking
{
    /// <summary>
    /// SET DE FUNCTII CU CARE SE CONSTRUIESC PACHETELE CARE VOR FI TRIMISE CATRE CLIENT
    /// </summary>
    public static class ServerSend
    {
        private static void SendTcpData(int toClient, Packet packet)
        {
            packet.InsertLength();
            Server.ClientConnections[toClient].Tcp.SendData(packet);
        }

        private static void SendTcpDataToAll(Packet packet)
        {
            packet.InsertLength();
            for (var i = 1; i <= Server.MaxPlayers; i++) Server.ClientConnections[i].Tcp.SendData(packet);
        }

        private static void SendTcpDataToAll(int exceptClient, Packet packet)
        {
            packet.InsertLength();
            for (var i = 1; i <= Server.MaxPlayers; i++) 
                if (i != exceptClient) 
                    Server.ClientConnections[i].Tcp.SendData(packet);
        }

        private static void SendUdpData(int toClient, Packet packet)
        {
            packet.InsertLength();
            Server.ClientConnections[toClient].Udp.SendData(packet);
        }
        
        private static void SendUdpDataToAll(Packet packet)
        {
            packet.InsertLength();
            for (var i = 1; i <= Server.MaxPlayers; i++) Server.ClientConnections[i].Udp.SendData(packet);
        }

        private static void SendUdpDataToAll(int exceptClient, Packet packet)
        {
            packet.InsertLength();
            for (var i = 1; i <= Server.MaxPlayers; i++) 
                if (i != exceptClient) 
                    Server.ClientConnections[i].Udp.SendData(packet);
        }
        
        public static void Welcome(int toClient, string message)
        {
            using (var packet = new Packet((int) ServerPackets.Welcome))
            {
                SendTcpData(toClient, packet.Write(message).Write(toClient));
            }
        }

        public static void SpawnPlayer(int toClient, ServerPlayerManager player)
        {
            using (var packet = new Packet((int)ServerPackets.SpawnPlayer))
            {
                SendTcpData(toClient, 
                    packet.Write(player.Id)
                        .Write(player.Username)
                        .Write(player.playerMovement.transform.position)
                        .Write(player.playerMovement.transform.rotation));
            }
        }

        public static void PlayerPosition(int id, PlayerMovement player)
        {
            using (var packet = new Packet((int) ServerPackets.PlayerPosition))
            {
                SendUdpDataToAll(packet.Write(id).Write(player.transform.position));
            }
        }
        
        public static void PlayerRotation(int id, PlayerMovement player)
        {
            using (var packet = new Packet((int) ServerPackets.PlayerRotation))
            {
                SendUdpDataToAll(id, packet.Write(id).Write(player.transform.rotation));
            }
        }

        public static void PlayerDisconnected(int playerId)
        {
            using (var packet = new Packet((int) ServerPackets.PlayerDisconnected))
            {
                SendTcpDataToAll(packet.Write(playerId));
            }
        }

        public static void PlayerHealth(int playerId, EntityHealth entityHealth)
        {
            using (var packet = new Packet((int) ServerPackets.PlayerHealth))
            {
                SendTcpDataToAll(packet.Write(playerId).Write(entityHealth.Health));
            }
        }

        public static void CreateItemSpawner(int toClient, int spawnerId, Vector3 position, bool hasItem, int itemId)
        {
            using (var packet = new Packet((int) ServerPackets.CreateItemSpawner))
            {
                SendTcpData(toClient, packet.Write(spawnerId).Write(position).Write(hasItem).Write(itemId));
            }
        }

        public static void ItemSpawned(int spawnerId)
        {
            using (var packet = new Packet((int) ServerPackets.ItemSpawned))
            {
                SendTcpDataToAll(packet.Write(spawnerId));
            }
        }
        
        public static void ItemPickedUp(int spawnerId, int byPlayer)
        {
            using (var packet = new Packet((int) ServerPackets.ItemPickedUp))
            {
                SendTcpDataToAll(packet.Write(spawnerId).Write(byPlayer));
            }
        }

        public static void AmmoPickedUp(int clientId, int amount)
        {
            using (var packet = new Packet((int) ServerPackets.AmmoPickedUp))
            {
                SendTcpData(clientId, packet.Write(clientId).Write(amount));
            }
        }

        public static void WeaponPickedUp(int clientId, int weaponId)
        {
            using (var packet = new Packet((int) ServerPackets.WeaponPickedUp))
            {
                SendTcpDataToAll(packet.Write(clientId).Write(weaponId));
            }
        }

        public static void HandWeaponUpdate(int clientId, int weaponIndex)
        {
            using (var packet = new Packet((int) ServerPackets.HandWeaponUpdate))
            {
                SendTcpDataToAll(clientId, packet.Write(clientId).Write(weaponIndex));
            }
        }
        
        public static void InitializeInventory(int toClient, int clientId, List<int> weapons)
        {
            using (var packet = new Packet((int) ServerPackets.InitializeInventory))
            {
                packet.Write(clientId);
                packet.Write(weapons.Count);
                foreach (var weapon in weapons) packet.Write(weapon);

                SendTcpData(toClient, packet);
            }
        }

        public static void SpawnEnemy(int clientId, EnemyManager enemyManager)
        {
            using (var packet = new Packet((int) ServerPackets.SpawnEnemy))
            {
                SendTcpData(clientId, packet
                    .Write(enemyManager.EnemyId)
                    .Write(enemyManager.enemyScriptableObject.Id)
                    .Write(enemyManager.transform.position)
                    .Write(enemyManager.transform.rotation));
            }
        }

        public static void EnemyHealth(int enemyId, EntityHealth entityHealth)
        {
            using (var packet = new Packet((int) ServerPackets.EnemyHealth))
            {
                SendTcpDataToAll(packet.Write(enemyId).Write(entityHealth.Health));
            }
        }

        public static void EnemyPositionAndRotation(int enemyId, Vector3 transformPosition, Quaternion transformRotation)
        {
            using (var packet = new Packet((int) ServerPackets.EnemyPositionAndRotation))
            {
                SendUdpDataToAll(packet.Write(enemyId).Write(transformPosition).Write(transformRotation));
            }
        }

        public static void SendEnemyState(int enemyId, int aiState)
        {
            using (var packet = new Packet((int) ServerPackets.SendEnemyState))
            {
                SendUdpDataToAll(packet.Write(enemyId).Write(aiState));
            }
        }
    }
}