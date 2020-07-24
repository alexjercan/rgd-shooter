using System;
using _Project.Scripts.Networking.ByteArray;
using _Project.Scripts.Networking.Packet;
using _Project.Scripts.Threading;
using UnityEngine;
using Logger = _Project.Scripts.Logging.Logger;

namespace _Project.Scripts.Networking
{
    public class TcpServerManager : MonoBehaviour
    {
        public int port = 26950;
        
        private TCP.Server _server;
        
        private void Awake()
        {
            if (GetComponents<TcpServerManager>().Length > 1)
                Logger.Error("Multiple ClientManager instances in the scene!");
        }

        private void Start()
        {
            _server = new TCP.Server(20, port);
            _server.SetReceivedDatagramHandler((sender, receivePacket) => MainThreadScheduler.EnqueueOnMainThread(() => ReadPacket(receivePacket)));
            _server.Listen();
        }
        
        private void ReadPacket(ByteArrayReader receivePacket)
        {
            var packetType = (ClientPacket)receivePacket.ReadInt();

            switch (packetType)
            {
                case ClientPacket.InvalidPacket:
                    break;
                case ClientPacket.WelcomeReceived:
                    HandleWelcomeReceived(receivePacket);
                    break;
                default:
                    return;
            }
        }
        
        private void HandleWelcomeReceived(ByteArrayReader byteArrayReader)
        {
            var (id, message) = PacketTemplates.WriteWelcomeReceivedPacket(byteArrayReader);
            
            Logger.Info($"TCP: received username: '{message}' of client {id}");
        }
    }
}