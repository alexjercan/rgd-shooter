using UnityEngine;

namespace _Project.Scripts.Util.Weapon
{
	public class BulletShoot : MonoBehaviour
	{
		public float despawnTime = 10f;

		private void Start()
		{
			Destroy(gameObject, despawnTime);
			//TODO: USE AN OBJECT POOL TO STORE ITEMS INSTEAD OF INSTANTIATING AND DESTROYING THEM FOR BETTER PERFORMANCE
		}
	}
}
