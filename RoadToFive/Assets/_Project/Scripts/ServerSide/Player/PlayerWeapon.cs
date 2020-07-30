using _Project.Scripts.Mechanics;
using _Project.Scripts.Util.Weapon;
using UnityEngine;

namespace _Project.Scripts.ServerSide.Player
{
    public class PlayerWeapon : MonoBehaviour
    {
        //public event EventHandler<Vector3> Shoot; 
        private Weapon _weapon;
        
        [SerializeField] private Transform shootOrigin;
        [SerializeField] private float maxBulletTravel;
        [SerializeField] private LayerMask layerMask;

        public void OnShoot(Vector3 direction)
        {
            if (!Physics.Raycast(shootOrigin.position, direction, out var hit, maxBulletTravel, layerMask)) return;
            //var hitPoint = hit.point;
            //Shoot?.Invoke(this, hitPoint); TODO this can be used for visual effects 
                
            hit.collider.GetComponent<EntityHealth>().Damage(_weapon.Damage);
        }

        public void SetActiveWeapon(Weapon weapon) => _weapon = weapon;
    }
}