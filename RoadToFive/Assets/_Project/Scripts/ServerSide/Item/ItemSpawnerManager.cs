using System.Collections.Generic;
using _Project.Scripts.Util.Item;
using UnityEngine;

namespace _Project.Scripts.ServerSide.Item
{
    public class ItemSpawnerManager : MonoBehaviour
    {
        public static readonly Dictionary<int, ServerItemSpawner> ItemSpawners = new Dictionary<int, ServerItemSpawner>();

        [SerializeField] private List<ItemScriptableObject> itemScriptableObjects;
        
        private readonly Dictionary<int, GameObject> _itemPrefabs = new Dictionary<int, GameObject>();
        
        private void Awake()
        {
            foreach (var itemScriptableObject in itemScriptableObjects)
                _itemPrefabs.Add(itemScriptableObject.id, itemScriptableObject.prefab);
        }
    }
}