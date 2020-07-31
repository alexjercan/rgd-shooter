using UnityEngine;

namespace _Project.Scripts.Util.Item
{
    public class ItemScriptableObject : ScriptableObject
    {
        public int Id { get; set; }
        
        public new string name;
        public ItemType itemType;
        public GameObject prefab;

        public enum ItemType
        {
            HealthKit,
            AmmoPack,
            Weapon,
        }
    }
}