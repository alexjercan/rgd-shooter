﻿using _Project.Scripts.Mechanics;
using _Project.Scripts.Util.Item;
using UnityEngine;

namespace _Project.Scripts.ServerSide.Player
{
    public class PlayerWeapon : MonoBehaviour //DONE DO NOT MODIFY
    {
        [SerializeField] private Transform shootOrigin;
        [SerializeField] private float maxBulletTravel;
        [SerializeField] private LayerMask layerMask;

        public void OnShoot(Vector3 direction, int weaponId)
        {
            if (!Physics.Raycast(shootOrigin.position, direction, out var hit, maxBulletTravel, layerMask)) return;

            var items = ServerManager.Instance.GetSpawnableItems();
            var weapon = ((WeaponScriptableObject) items[weaponId]);
            hit.collider.GetComponent<EntityHealth>().Damage(weapon.damage);
        }
    }
}