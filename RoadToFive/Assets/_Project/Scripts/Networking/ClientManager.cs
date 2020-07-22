using System.Net.Sockets;
using _Project.Scripts.Networking.TCP;
using UnityEngine;
using Logger = _Project.Scripts.Logging.Logger;

namespace _Project.Scripts.Networking
{
    public class ClientManager : MonoBehaviour
    {
        private Client _client;
        
        public string dummyIp = "127.0.0.1";
        public int port = 26950;

        private void Awake()
        {
            if (GetComponents<ClientManager>().Length > 1)
                Logger.Error("Multiple ClientManager instances in the scene!");
        }

        private void Start()
        {
            _client = new Client();
            
            _client.ConnectToServer(dummyIp, port);
        }
    }
}