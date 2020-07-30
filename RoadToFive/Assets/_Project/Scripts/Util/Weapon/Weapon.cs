using UnityEngine;

namespace _Project.Scripts.Util.Weapon
{
    [CreateAssetMenu(fileName = "Items", menuName = "Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        public int damage;
    }
}