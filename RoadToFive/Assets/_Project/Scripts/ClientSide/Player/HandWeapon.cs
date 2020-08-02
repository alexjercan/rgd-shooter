using System.Collections.Generic;
using _Project.Scripts.Util.Item;
using _Project.Scripts.Util.Weapon;
using UnityEngine;

namespace _Project.Scripts.ClientSide.Player
{
    public class HandWeapon : MonoBehaviour //DONE DO NOT MODIFY
    {
        [SerializeField] private Transform handTransform;
        
        public readonly Dictionary<int, WeaponManager> WeaponManagers = new Dictionary<int, WeaponManager>();
        public int MainWeaponId { get; private set; }

        public void SetWeaponTo(ItemScriptableObject weapon)
        {
            var weaponId = weapon.Id;
            
            if (WeaponManagers.ContainsKey(MainWeaponId)) WeaponManagers[MainWeaponId].gameObject.SetActive(false);
            if (WeaponManagers.ContainsKey(weaponId))
            {
                WeaponManagers[weaponId].gameObject.SetActive(true);
            }
            else
            {
                var items = GameManager.Instance.spawnableItems.GetSpawnableItems();
                var scriptableObject = items[weaponId];
                if (scriptableObject.ItemType != ItemScriptableObject.Type.Weapon) return;

                var weaponManagerPrefab = (WeaponScriptableObject) scriptableObject;
                var weaponManager = Instantiate(weaponManagerPrefab.handWeaponManagerPrefab, handTransform, false);
                WeaponManagers.Add(weaponId, weaponManager);
            }

            MainWeaponId = weaponId;
        }
    }
}