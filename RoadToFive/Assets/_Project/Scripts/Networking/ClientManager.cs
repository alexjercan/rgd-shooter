using UnityEngine;
using Logger = _Project.Scripts.Logging.Logger;

namespace _Project.Scripts.Networking
{
    public class ClientManager : MonoBehaviour
    {
        public string dummyIp = "127.0.0.1";
        public int port = 26950;

        private void Awake()
        {
            if (GetComponents<ClientManager>().Length > 1)
                Logger.Error("Multiple ClientManager instances in the scene!");
        }

        private void Start()
        {
            //var clientTcp = new ClientTcp(dummyIp, port);
            var clientUdp = new ClientUdp(dummyIp, port);
            
            //clientTcp.ConnectToServer();
            clientUdp.BindToServer(16483);
        }
    }
}