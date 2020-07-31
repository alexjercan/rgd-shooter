using UnityEngine;

namespace _Project.Scripts.Util.Item
{
    public abstract class ItemScriptableObject : ScriptableObject
    {
        public int Id { get; set; }
        public abstract Type ItemType { get; }
        
        public new string name;
        public GameObject prefab;

        public enum Type
        {
            HealthKit,
            AmmoPack,
            Weapon,
        }
    }
}