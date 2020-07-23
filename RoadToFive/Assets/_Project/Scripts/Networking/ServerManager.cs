using UnityEngine;
using Logger = _Project.Scripts.Logging.Logger;

namespace _Project.Scripts.Networking
{
    public class ServerManager : MonoBehaviour
    {
        public int port = 26950;
        
        private void Awake()
        {
            if (GetComponents<ServerManager>().Length > 1)
                Logger.Error("Multiple ClientManager instances in the scene!");
        }
        
        private void Start()
        {
            //var serverTcp = new ServerTcp(20, port);
            var serverUdp = new ServerUdp(20, port);
            
            serverUdp.Listen();
            //serverTcp.Start();
        }
    }
}