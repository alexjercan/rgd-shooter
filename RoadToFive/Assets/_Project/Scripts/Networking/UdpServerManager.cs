using System;
using _Project.Scripts.Networking.ByteArray;
using _Project.Scripts.Networking.Datagram;
using _Project.Scripts.Threading;
using UnityEngine;
using Logger = _Project.Scripts.Logging.Logger;

namespace _Project.Scripts.Networking
{
    public class UdpServerManager : MonoBehaviour
    {
        public int port = 26950;
        
        private void Awake()
        {
            if (GetComponents<UdpServerManager>().Length > 1)
                Logger.Error("Multiple ClientManager instances in the scene!");
        }
        
        private void Start()
        {
            var server = new UDP.Server(port);
            server.ReceivedDatagram +=(sender, receiveDatagram) => MainThreadScheduler.EnqueueOnMainThread(() => ReadDatagram(receiveDatagram));

            server.Listen();
        }
        
        private void ReadDatagram(ByteArrayReader receiveDatagram)
        {
            var datagramType = (ClientDatagram)receiveDatagram.ReadInt();

            switch (datagramType)
            {
                case ClientDatagram.InvalidDatagram:
                    break;
                case ClientDatagram.WelcomeReceived:
                    HandleWelcomeReceived(receiveDatagram);
                    break;
                default:
                    return;
            }
        }
        
        private void HandleWelcomeReceived(ByteArrayReader receiveDatagram)
        {
            var (id, message)  = DatagramTemplates.ReadWelcomeReceivedMessage(receiveDatagram);

            Logger.Info($"UDP: received username: '{message}' of client {id}");
        }
    }
}