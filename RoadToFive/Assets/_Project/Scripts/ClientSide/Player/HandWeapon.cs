using System.Collections.Generic;
using _Project.Scripts.Util.Item;
using _Project.Scripts.Util.Weapon;
using UnityEngine;

namespace _Project.Scripts.ClientSide.Player
{
    public class HandWeapon : MonoBehaviour //DONE DO NOT MODIFY
    {
        [SerializeField] private Transform handTransform;
        
        private readonly Dictionary<int, WeaponManager> _weaponManagers = new Dictionary<int, WeaponManager>();
        private int _mainWeaponId;

        public void SetWeaponTo(ItemScriptableObject weapon)
        {
            var weaponId = weapon.Id;
            
            if (_weaponManagers.ContainsKey(_mainWeaponId)) _weaponManagers[_mainWeaponId].gameObject.SetActive(false);
            if (_weaponManagers.ContainsKey(weaponId))
            {
                _weaponManagers[weaponId].gameObject.SetActive(true);
            }
            else
            {
                var items = GameManager.Instance.spawnableItems.GetSpawnableItems();
                var scriptableObject = items[weaponId];
                if (scriptableObject.ItemType != ItemScriptableObject.Type.Weapon) return;

                var weaponManagerPrefab = (WeaponScriptableObject) scriptableObject;
                var weaponManager = Instantiate(weaponManagerPrefab.handWeaponManagerPrefab, handTransform, false);
                _weaponManagers.Add(weaponId, weaponManager);
            }

            _mainWeaponId = weaponId;
        }

        public int GetHandWeaponId() => _mainWeaponId;
    }
}