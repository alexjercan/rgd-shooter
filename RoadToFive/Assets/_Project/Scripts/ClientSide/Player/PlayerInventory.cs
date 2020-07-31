using System.Collections.Generic;
using _Project.Scripts.Util.Item;
using UnityEngine;

namespace _Project.Scripts.ClientSide.Player
{
    public class PlayerInventory : MonoBehaviour
    {
        private readonly Dictionary<int, ItemScriptableObject> _weapons = new Dictionary<int, ItemScriptableObject>();
        public int Ammo { get; private set; }

        public void AddAmmo(int amount)
        {
            Ammo += amount;
        }

        public void AddWeapon(int weaponId)
        {
            var weapon = GameManager.Instance.items[weaponId];
            if (_weapons.ContainsKey(weaponId) || weapon.ItemType != ItemScriptableObject.Type.Weapon) return;

            _weapons.Add(weaponId, weapon);
        }
    }
}