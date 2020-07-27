using UnityEngine;

namespace _Project.Scripts.Networking.ServerSide
{
    public class ServerManager : MonoBehaviour
    {
        private void Start()
        {
            Server.Start(20, 26950);
        }
    }
}