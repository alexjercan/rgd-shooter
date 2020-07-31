using UnityEngine;

namespace _Project.Scripts.Util.Threading
{
    public class MainThreadManager : MonoBehaviour //DONE DO NOT MODIFY
    {
        private void FixedUpdate() => MainThreadScheduler.UpdateMainThread();
    }
}