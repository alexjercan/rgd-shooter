using UnityEngine;

namespace _Project.Scripts.Util.Weapon
{
    [CreateAssetMenu(fileName = "Items", menuName = "Weapon", order = 0)]
    public class WeaponScriptableObject : ScriptableObject
    {
        public GameObject prefab;
        public int damage;
    }
}