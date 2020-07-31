using UnityEngine;

namespace _Project.Scripts.Util.Item
{
    [CreateAssetMenu(fileName = "new ammo", menuName = "Item/Ammo", order = 0)]
    public class AmmoPackScriptableObject : ItemScriptableObject
    {
        public override Type ItemType => Type.AmmoPack;
        public int ammoAmount;
    }
}