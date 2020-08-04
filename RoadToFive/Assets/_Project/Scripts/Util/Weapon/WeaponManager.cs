using UnityEngine;

namespace _Project.Scripts.Util.Weapon
{
    public class WeaponManager : MonoBehaviour
    {
        public Transform barrelEndPoint;
        public GameObjectDespawner bulletShootPrefab;

        public void ShootWeapon()
        {
            var bulletShoot = Instantiate(bulletShootPrefab, barrelEndPoint.position, barrelEndPoint.rotation);
        }
    }
}