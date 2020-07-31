using UnityEngine;

namespace _Project.Scripts.ClientSide.Player
{
    public class PlayerInventory : MonoBehaviour
    {
        public int Ammo { get; private set; }

        public void AddAmmo(int amount)
        {
            Ammo += amount;
        }
    }
}