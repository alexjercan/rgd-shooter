using System.Collections.Generic;
using _Project.Scripts.Util.Item;
using UnityEngine;

namespace _Project.Scripts.ClientSide.Player
{
    public class PlayerInventory : MonoBehaviour
    {
        private readonly List<ItemScriptableObject> _weaponList = new List<ItemScriptableObject>();
        private int _ammo;

        public void AddAmmo(int amount) => _ammo += amount;

        public void AddWeapon(int weaponId)
        {
            var items = GameManager.Instance.GetSpawnableItems();
            var weapon = items[weaponId];
            if (_weaponList.Contains(weapon)|| weapon.ItemType != ItemScriptableObject.Type.Weapon) return;
            _weaponList.Add(weapon);
        }

        public ItemScriptableObject GetWeaponAtIndex(int index) => _weaponList[index];

        public int GetWeaponCount() => _weaponList.Count;

        public int GetAmmo() => _ammo;
    }
}