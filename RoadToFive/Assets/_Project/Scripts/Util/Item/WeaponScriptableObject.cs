﻿using UnityEngine;

namespace _Project.Scripts.Util.Item
{
    [CreateAssetMenu(fileName = "new weapon", menuName = "Item/Weapon", order = 0)]
    public class WeaponScriptableObject : ItemScriptableObject
    {
        public int damage;
    }
}