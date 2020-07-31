using System.Collections.Generic;
using _Project.Scripts.ServerSide.Networking;
using _Project.Scripts.Util.Item;
using UnityEngine;

namespace _Project.Scripts.ServerSide.Item
{
    public class ItemSpawnerManager : MonoBehaviour
    {
        public static readonly Dictionary<int, ServerItemSpawner> ItemSpawners = new Dictionary<int, ServerItemSpawner>();

        [SerializeField] private List<ItemScriptableObject> itemScriptableObjects;
        
        private static readonly Dictionary<int, ItemScriptableObject> _itemScriptableObjects = new Dictionary<int, ItemScriptableObject>();
        
        private void Awake()
        {
            foreach (var itemScriptableObject in itemScriptableObjects)
                _itemScriptableObjects.Add(itemScriptableObject.id, itemScriptableObject);
        }

        public static void ItemSpawned(int spawnerId) => ServerSend.ItemSpawned(spawnerId);

        public static void ItemPickedUp(int spawnerId, int clientId) => ServerSend.ItemPickedUp(spawnerId, clientId);
    }
}