using System.Collections.Generic;
using _Project.Scripts.Util.Item;
using UnityEngine;

namespace _Project.Scripts.ServerSide.Player
{
    public class PlayerInventory : MonoBehaviour
    {
        private readonly List<int> _weaponIds = new List<int>();
        
        public void AddWeapon(int weaponId)
        {
            var items = ServerManager.Instance.GetSpawnableItems();
            var weapon = items[weaponId];
            if (_weaponIds.Contains(weaponId)|| weapon.ItemType != ItemScriptableObject.Type.Weapon) return;
            _weaponIds.Add(weaponId);
        }

        public List<int> GetWeapons() => _weaponIds;
    }
}