using UnityEngine;

namespace _Project.Scripts.Util.Weapon
{
    [CreateAssetMenu(fileName = "new weapon", menuName = "Item/Weapon", order = 0)]
    public class WeaponScriptableObject : ScriptableObject
    {
        public int id;
        public GameObject prefab;
        public int damage;
    }
}