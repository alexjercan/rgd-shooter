using UnityEngine;

namespace _Project.Scripts.Util.Item
{
    [CreateAssetMenu(fileName = "new ammo", menuName = "Item/Ammo", order = 0)]
    public class AmmoPackScriptableObject : ItemScriptableObject
    {
        public int ammoAmount;
    }
}