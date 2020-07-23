using _Project.Scripts.Networking.ByteArray;
using _Project.Scripts.Networking.Datagram;
using UnityEngine;
using Logger = _Project.Scripts.Logging.Logger;

namespace _Project.Scripts.Networking
{
    public class ClientManager : MonoBehaviour
    {
        public string dummyIp = "127.0.0.1";
        public int remotePort = 26950;
        public int localPort = 16483;

        private UDP.Client _client;
        
        private void Awake()
        {
            if (GetComponents<ClientManager>().Length > 1)
                Logger.Error("Multiple ClientManager instances in the scene!");
        }

        private void Start()
        {
            _client = new UDP.Client(dummyIp, remotePort, localPort);
            
            _client.SendDatagram(DatagramTemplates.WriteDummyMessage());
        }
    }
}