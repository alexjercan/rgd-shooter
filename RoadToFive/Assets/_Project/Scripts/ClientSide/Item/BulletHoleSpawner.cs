using _Project.Scripts.Util.Weapon;
using UnityEngine;

namespace _Project.Scripts.ClientSide.Item
{
    public class BulletHoleSpawner : MonoBehaviour
    {
        [SerializeField] private GameObjectDespawner[] bulletHolePrefabs;

        public void SpawnBulletHole(Vector3 hitPosition, Vector3 hitNormal)
        {
            var hitRotation = Quaternion.LookRotation(hitNormal);

            var prefab = bulletHolePrefabs[Random.Range(0, bulletHolePrefabs.Length)];
            var bulletHoleGameObject = Instantiate(prefab, hitPosition + hitNormal * 0.01f, hitRotation);
        }
    }
}