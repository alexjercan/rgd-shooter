﻿using _Project.Scripts.Util.DataStructure;
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
                packet.Write(Client.MyId).Write("GUEST " + Client.MyId);
                
                SendTcpData(packet);
            }
        }

        public static void PlayerMovement(Vector3 movementInput, Quaternion rotation)
        {
            using (var packet = new Packet((int) ClientPackets.PlayerMovement))
            {
                packet.Write(movementInput).Write(rotation);

                SendUdpData(packet);
            }
        }
    }
}