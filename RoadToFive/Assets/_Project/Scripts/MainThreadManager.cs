using _Project.Scripts.Networking.Threading;
using UnityEngine;

namespace _Project.Scripts
{
    public class MainThreadManager : MonoBehaviour
    {
        private void Update() => MainThreadScheduler.UpdateMainThread();
    }
}