using UnityEngine;

namespace _Project.Scripts.Util.Threading
{
    public class MainThreadManager : MonoBehaviour
    {
        private void FixedUpdate() => MainThreadScheduler.UpdateMainThread();
    }
}