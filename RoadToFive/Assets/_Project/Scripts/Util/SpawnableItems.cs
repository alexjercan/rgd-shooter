using System.Collections.Generic;
using _Project.Scripts.Util.Item;
using UnityEngine;

namespace _Project.Scripts.Util
{
    public class SpawnableItems : MonoBehaviour
    {
        [SerializeField] public List<ItemScriptableObject> items = new List<ItemScriptableObject>();

        public List<ItemScriptableObject> GetSpawnableItems() => items;

        private void Start()
        {
            for (var i = 0; i < items.Count; i++) items[i].Id = i;
        }
    }
}