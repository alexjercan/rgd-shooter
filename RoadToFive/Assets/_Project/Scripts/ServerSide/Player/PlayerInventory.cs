using System;
using System.Collections.Generic;
using _Project.Scripts.Util.Weapon;
using UnityEngine;

namespace _Project.Scripts.ServerSide.Player
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private List<WeaponScriptableObject> weapons;
        private readonly List<Weapon> _weapons = new List<Weapon>();

        private void Awake()
        {
            foreach (var weapon in weapons) _weapons.Add(new Weapon(weapon));
        }

        public Weapon GetWeapon(int index) => _weapons[index];
    }
}