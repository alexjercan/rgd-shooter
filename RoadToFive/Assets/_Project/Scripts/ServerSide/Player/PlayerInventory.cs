using UnityEngine;

namespace _Project.Scripts.ServerSide.Player
{
    public class PlayerInventory : MonoBehaviour
    {
        private int _ammo = 0;

        public void AddAmmo(int amount) => _ammo += amount;
    }
}