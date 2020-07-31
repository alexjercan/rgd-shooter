using UnityEngine;

namespace _Project.Scripts.Util.Item
{
    public class ItemScriptableObject : ScriptableObject
    {
        public int id;
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