using System;
using System.Collections.Generic;
using _Project.Scripts.Util.Item;
using UnityEngine;

namespace _Project.Scripts.ClientSide.Player
{
    public class PlayerInventory : MonoBehaviour //DONE DO NOT MODIFY
    {
        public event EventHandler<PlayerInventory> AmmoChanged;
        public int Ammo { get; private set; }

        private readonly List<ItemScriptableObject> _weaponList = new List<ItemScriptableObject>();

        public void AddAmmo(int amount)
        {
            Ammo += amount;
            AmmoChanged?.Invoke(this, this);
        }

        public void AddWeapon(int weaponId)
        {
            var items = GameManager.Instance.spawnableItems.GetSpawnableItems();
            var weapon = items[weaponId];
            if (_weaponList.Contains(weapon)|| weapon.ItemType != ItemScriptableObject.Type.Weapon) return;
            _weaponList.Add(weapon);
        }

        public ItemScriptableObject GetWeaponAtIndex(int index) => _weaponList[index];

        public int GetWeaponCount() => _weaponList.Count;

        public int GetAmmo() => Ammo;
    }
}