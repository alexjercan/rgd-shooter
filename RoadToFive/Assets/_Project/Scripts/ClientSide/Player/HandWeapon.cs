using System.Collections.Generic;
using _Project.Scripts.Util.Item;
using UnityEngine;

namespace _Project.Scripts.ClientSide.Player
{
    public class HandWeapon : MonoBehaviour
    {
        [SerializeField] private Transform handTransform;
        
        private readonly Dictionary<int, GameObject> _weaponsVisuals = new Dictionary<int, GameObject>();
        private int _mainWeaponId;

        public void SetWeaponTo(ItemScriptableObject weapon)
        {
            var weaponId = weapon.Id;
            
            if (_weaponsVisuals.ContainsKey(_mainWeaponId)) _weaponsVisuals[_mainWeaponId].SetActive(false);
            if (_weaponsVisuals.ContainsKey(weaponId))
            {
                _weaponsVisuals[weaponId].SetActive(true);
            }
            else
            {
                var scriptableObject = GameManager.Instance.items[weaponId];
                if (scriptableObject.ItemType != ItemScriptableObject.Type.Weapon) return;
                
                _weaponsVisuals.Add(weaponId, Instantiate(scriptableObject.prefab, handTransform, false));
            }

            _mainWeaponId = weaponId;
        }

        public int GetHandWeaponId() => _mainWeaponId;
    }
}