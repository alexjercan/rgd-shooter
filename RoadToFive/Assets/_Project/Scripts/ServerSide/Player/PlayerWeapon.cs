using System;
using UnityEngine;

namespace _Project.Scripts.ServerSide.Player
{
    public class PlayerWeapon : MonoBehaviour
    {
        //public event EventHandler<Vector3> Shoot; 
        
        [SerializeField] private Transform shootOrigin;
        [SerializeField] private float maxBulletTravel;
        [SerializeField] private int layerMask;

        public void OnShoot(Vector3 direction)
        {
            if (Physics.Raycast(shootOrigin.position, direction, out var hit, maxBulletTravel, layerMask))
            {
                var hitPoint = hit.point;
                
                //Shoot?.Invoke(this, hitPoint);
                
                
            }
        }
    }
}