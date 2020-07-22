using UnityEngine;

namespace _Project.Scripts.Networking
{
    public class ServerManager : MonoBehaviour
    {
        private void Start()
        {
            var server = new Server(20, 26950);

            server.Start();
        }
    }
}