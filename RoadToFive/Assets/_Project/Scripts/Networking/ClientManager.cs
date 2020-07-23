using System.Net;
using System.Net.Sockets;
using _Project.Scripts.Networking.Datagram;
using UnityEngine;
using Logger = _Project.Scripts.Logging.Logger;

namespace _Project.Scripts.Networking
{
    public class ClientManager : MonoBehaviour
    {
        public string dummyIp = "127.0.0.1";
        public int remotePort = 26950;

        private void Awake()
        {
            if (GetComponents<ClientManager>().Length > 1)
                Logger.Error("Multiple ClientManager instances in the scene!");
        }

        private void Start()
        {
            var clientTcp = new TCP.Client(dummyIp, remotePort);
            var clientUdp = new UDP.Client(dummyIp, remotePort, clientTcp.Port);
            
            clientUdp.SendDatagram(DatagramTemplates.WriteDummyMessage());
        }
    }
}