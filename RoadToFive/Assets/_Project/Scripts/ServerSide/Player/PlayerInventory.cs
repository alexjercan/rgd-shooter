using System.Collections.Generic;
using _Project.Scripts.Util.Item;
using UnityEngine;

namespace _Project.Scripts.ServerSide.Player
{
    public class PlayerInventory : MonoBehaviour //DONE DO NOT MODIFY
    {
        private readonly List<int> _weaponIds = new List<int>();
        private int _handWeaponIndex = -1;
        
        public void AddWeapon(int weaponId)
        {
            var items = ServerManager.Instance.spawnableItems.GetSpawnableItems();
            var weapon = items[weaponId];
            if (_weaponIds.Contains(weaponId)|| weapon.ItemType != ItemScriptableObject.Type.Weapon) return;
            _weaponIds.Add(weaponId);
        }

        public List<int> GetWeapons() => _weaponIds;

        public void SetHandWeaponIndex(int index) => _handWeaponIndex = index;

        public int GetHandWeaponIndex() => _handWeaponIndex;
    }
}