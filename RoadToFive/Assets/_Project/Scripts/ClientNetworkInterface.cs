using System;
using _Project.Scripts.Networking;
using UnityEngine;
using Logger = _Project.Scripts.Logging.Logger;

namespace _Project.Scripts
{
    public class ClientNetworkInterface : MonoBehaviour
    {
        private ClientManager _clientManager;
        private void Awake()
        {
            if (GetComponents<ClientNetworkInterface>().Length > 1) 
                Logger.Error("Multiple ClientNetworkInterface instances in the scene!");
        }

        private void Start()
        {
            _clientManager = new ClientManager();
        }
    }
}