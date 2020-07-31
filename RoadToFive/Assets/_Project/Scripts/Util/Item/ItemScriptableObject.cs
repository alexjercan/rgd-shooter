using UnityEngine;

namespace _Project.Scripts.Util.Item
{
    [CreateAssetMenu(fileName = "new item", menuName = "Item/SpawnItem", order = 0)]
    public class ItemScriptableObject : ScriptableObject
    {
        public int id;
        public GameObject prefab;
    }
}