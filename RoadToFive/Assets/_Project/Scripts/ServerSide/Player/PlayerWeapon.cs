using System;
using _Project.Scripts.Mechanics;
using _Project.Scripts.Util.Item;
using UnityEngine;

namespace _Project.Scripts.ServerSide.Player
{
    public class PlayerWeapon : MonoBehaviour //DONE DO NOT MODIFY
    {
        public event EventHandler<RaycastHit> BulletHit;
    
        [SerializeField] private Transform shootOrigin;
        [SerializeField] private float maxBulletTravel;
        [SerializeField] private LayerMask layerMask;

        public void OnShoot(Vector3 direction, int weaponId)
        {
            if (!Physics.Raycast(shootOrigin.position, direction, out var hit, maxBulletTravel, layerMask)) return;
            
            var items = ServerManager.Instance.spawnableItems.GetSpawnableItems();
            var weapon = (WeaponScriptableObject) items[weaponId];
            if (hit.collider.TryGetComponent<EntityHealth>(out var entityHealth)) entityHealth.Damage(weapon.damage);
            else BulletHit?.Invoke(this, hit);
        }
    }
}