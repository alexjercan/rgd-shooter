using System;
using _Project.Scripts.Networking.ByteArray;
using _Project.Scripts.Networking.Packet;
using _Project.Scripts.Threading;
using UnityEngine;
using Logger = _Project.Scripts.Logging.Logger;

namespace _Project.Scripts.Networking
{
    public class TcpClientManager : MonoBehaviour
    {
        public string remoteIp = "127.0.0.1";
        public int remotePort = 26950;

        private TCP.Client _client;
        
        private void Awake()
        {
            if (GetComponents<TcpClientManager>().Length > 1)
                Logger.Error("Multiple ClientManager instances in the scene!");
        }
        
        private void Start()
        {
            _client = new TCP.Client(remoteIp, remotePort);
            _client.SetReceivedDatagramHandler((sender, receivePacket) => MainThreadScheduler.EnqueueOnMainThread(() => ReadPacket(receivePacket)));
            _client.Connect();
        }

        private void ReadPacket(ByteArrayReader receivePacket)
        {
            var packetType = (ServerPacket)receivePacket.ReadInt();

            switch (packetType)
            {
                case ServerPacket.InvalidPacket:
                    break;
                case ServerPacket.WelcomePacket:
                    HandleWelcomePacket(receivePacket);
                    break;
                default:
                    return;
            }
        }
        
        private void HandleWelcomePacket(ByteArrayReader byteArrayReader)
        {
            var (clientId, message) = PacketTemplates.ReadWelcomePacket(byteArrayReader);

            Logger.Info($"TCP: received '{message}' from server!");
            
            _client.Id = clientId;
            _client.SendPacket(PacketTemplates.WriteWelcomeReceivedPacket(_client.Id, "guest " + _client.Id));
        }
    }
}