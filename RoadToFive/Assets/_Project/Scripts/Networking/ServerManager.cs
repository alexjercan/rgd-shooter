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
            var server = new UDP.Server(port);
        }
    }
}