using _Project.Scripts.Threading;
using UnityEngine;

namespace _Project.Scripts
{
    public class MainThreadManager : MonoBehaviour
    {
        private void Update() => MainThreadScheduler.UpdateMainThread();
    }
}