using System.Net;
using _Project.Scripts.Networking.ByteArray;
using _Project.Scripts.Networking.Datagram;
using _Project.Scripts.Threading;
using UnityEngine;
using Logger = _Project.Scripts.Logging.Logger;

namespace _Project.Scripts.Networking
{
    public class UdpClientManager : MonoBehaviour
    {
        public string remoteIp = "127.0.0.1";
        public int remotePort = 26950;

        private UDP.Client _client;

        private void Awake()
        {
            if (GetComponents<UdpClientManager>().Length > 1)
                Logger.Error("Multiple ClientManager instances in the scene!");
        }

        private void Start()
        {
            _client = new UDP.Client(remoteIp, remotePort, new IPEndPoint(IPAddress.Any, 0).Port);
            _client.ReceivedDatagram +=(sender, receiveDatagram) => MainThreadScheduler.EnqueueOnMainThread(() => ReadDatagram(receiveDatagram));

            _client.Listen();
            _client.SendDatagram(DatagramTemplates.WriteDummyMessage());
        }
        
        private void ReadDatagram(ByteArrayReader receiveDatagram)
        {
            var datagramType = (ServerDatagram)receiveDatagram.ReadInt();

            switch (datagramType)
            {
                case ServerDatagram.InvalidDatagram:
                    break;
                case ServerDatagram.WelcomeDatagram:
                    HandleWelcomeDatagram(receiveDatagram);
                    break;
                default:
                    return;
            }
        }
        
        private void HandleWelcomeDatagram(ByteArrayReader receiveDatagram)
        {
            _client.Id = DatagramTemplates.ReadWelcomeMessage(receiveDatagram);
            Logger.Info($"UDP: set this client's id to {_client.Id}");
            
            _client.SendDatagram(DatagramTemplates.WriteWelcomeReceivedMessage(_client.Id, "guest " + _client.Id));
        }
    }
}