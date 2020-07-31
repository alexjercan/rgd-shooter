using UnityEngine;

namespace _Project.Scripts.Util.Item
{
    [CreateAssetMenu(fileName = "new item", menuName = "Item/SpawnItem", order = 0)]
    public class ItemScriptableObject : ScriptableObject
    {
        public enum ItemType
        {
            HealthKit,
            AmmoPack,
        }
        
        public int id;
        public ItemType itemType;
        public GameObject prefab;
    }
}