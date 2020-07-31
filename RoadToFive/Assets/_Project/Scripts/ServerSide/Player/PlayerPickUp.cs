using System.Collections.Generic;
using _Project.Scripts.Util.Item;
using UnityEngine;

namespace _Project.Scripts.ServerSide.Player
{
    public class PlayerPickUp : MonoBehaviour
    {
        private delegate void PickUpHandler(ServerPlayerManager playerManager, ItemScriptableObject itemData);

        private static readonly Dictionary<int, PickUpHandler> PickUpHandlers = new Dictionary<int, PickUpHandler>()
        {
            {(int)ItemScriptableObject.ItemType.HealthKit, HealthKit},
            {(int)ItemScriptableObject.ItemType.AmmoPack, AmmoPack},
        };

        public void PickUpItem(ServerPlayerManager playerManager, ItemScriptableObject itemData) => PickUpHandlers[(int)itemData.itemType](playerManager, itemData);

        private static void HealthKit(ServerPlayerManager playerManager, ItemScriptableObject itemData) => 
            playerManager.HealPlayer(itemData.amount);

        private static void AmmoPack(ServerPlayerManager playerManager, ItemScriptableObject itemData)
        {
            
        }
    }
}