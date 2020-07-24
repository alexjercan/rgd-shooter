using _Project.Scripts.Networking;
using UnityEngine;
using Logger = _Project.Scripts.Logging.Logger;

namespace _Project.Scripts
{
    public class ServerNetworkInterface : MonoBehaviour
    {
        private ServerManager _serverManager;

        private void Awake()
        {
            if (GetComponents<ServerNetworkInterface>().Length > 1) 
                Logger.Error("Multiple ServerManager instances in the scene!");
        }

        private void Start()
        {
            _serverManager = new ServerManager();
        }
    }
}