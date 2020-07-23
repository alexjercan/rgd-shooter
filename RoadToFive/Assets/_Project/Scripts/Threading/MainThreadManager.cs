using System;
using UnityEngine;

namespace _Project.Scripts.Threading
{
    public class MainThreadManager : MonoBehaviour
    {
        private void Update() => MainThreadScheduler.UpdateMainThread();
    }
}