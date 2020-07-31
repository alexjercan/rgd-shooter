using UnityEngine;

namespace _Project.Scripts.Util.Item
{
    [CreateAssetMenu(fileName = "new health kit", menuName = "Item/Health", order = 0)]
    public class HealthKitScriptableObject : ItemScriptableObject
    {
        public override Type ItemType => Type.HealthKit;
        public int healAmount;
    }
}