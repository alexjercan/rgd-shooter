using _Project.Scripts.Mechanics;
using UnityEngine;

namespace _Project.Scripts.ServerSide.Player
{
    public class PlayerWeapon : MonoBehaviour
    {
        [SerializeField] private Transform shootOrigin;
        [SerializeField] private float maxBulletTravel;
        [SerializeField] private LayerMask layerMask;

        public void OnShoot(Vector3 direction, int weaponId)
        {
            if (!Physics.Raycast(shootOrigin.position, direction, out var hit, maxBulletTravel, layerMask)) return;

            hit.collider.GetComponent<EntityHealth>().Damage(ServerManager.Instance.weaponScriptableObjects[weaponId].damage);
        }
    }
}