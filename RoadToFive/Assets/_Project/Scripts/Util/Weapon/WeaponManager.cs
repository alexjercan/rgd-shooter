using UnityEngine;

namespace _Project.Scripts.Util.Weapon
{
    public class WeaponManager : MonoBehaviour
    {
        public Transform barrelEndPoint;
        public BulletShoot bulletShootPrefab;

        public void ShootWeapon()
        {
            var bulletShoot = Instantiate(bulletShootPrefab, barrelEndPoint.position, barrelEndPoint.rotation);
        }
    }
}