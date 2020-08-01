using _Project.Scripts.Util.DataStructure;
using UnityEngine;

namespace _Project.Scripts.ClientSide.Networking
{
    /// <summary>
    /// SET DE FUNCTII CU CARE SE CONSTRUIESC PACHETELE CARE VOR FI TRIMISE CATRE SERVER
    /// </summary>
    public static class ClientSend
    {
        private static void SendTcpData(Packet packet)
        {
            packet.InsertLength();
            Client.Connection.Tcp.SendData(packet);
        }

        private static void SendUdpData(Packet packet)
        {
            packet.InsertLength();
            Client.Connection.Udp.SendData(packet);
        }
        
        public static void WelcomeReceived()
        {
            using (var packet = new Packet((int) ClientPackets.WelcomeReceived))
            {
                SendTcpData(packet.Write(Client.MyId).Write("GUEST " + Client.MyId));
            }
        }

        public static void PlayerMovement(Vector3 movementInput, Quaternion rotation)
        {
            using (var packet = new Packet((int) ClientPackets.PlayerMovement))
            {
                SendUdpData(packet.Write(movementInput).Write(rotation));
            }
        }

        public static void PlayerShoot(Vector3 direction, int weaponId)
        {
            var playerManager = GameManager.Instance.GetPlayerManager(Client.MyId);

            if (playerManager.playerInventory.GetAmmo() <= 0) return;
            
            using (var packet = new Packet((int) ClientPackets.PlayerShoot))
            {
                SendTcpData(packet.Write(direction).Write(weaponId));
            }
            
            playerManager.playerInventory.AddAmmo(-1);
        }

        public static void HandWeapon(int weaponIndex)
        {
            using (var packet = new Packet((int) ClientPackets.HandWeapon))
            {
                SendTcpData(packet.Write(weaponIndex));
            }

            var playerManager = GameManager.Instance.GetPlayerManager(Client.MyId);
            
            playerManager.handWeapon.SetWeaponTo(playerManager.playerInventory.GetWeaponAtIndex(weaponIndex));
        }
    }
}