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
            {(int)ItemScriptableObject.Type.HealthKit, HealthKit},
            {(int)ItemScriptableObject.Type.AmmoPack, AmmoPack},
            {(int)ItemScriptableObject.Type.Weapon, Weapon},
        };

        public void PickUpItem(ServerPlayerManager playerManager, ItemScriptableObject itemData) => 
            PickUpHandlers[(int)itemData.ItemType](playerManager, itemData);

        private static void HealthKit(ServerPlayerManager playerManager, ItemScriptableObject itemData) => 
            playerManager.HealPlayer(((HealthKitScriptableObject)itemData).healAmount);

        private static void AmmoPack(ServerPlayerManager playerManager, ItemScriptableObject itemData) => 
            playerManager.AddAmmo(((AmmoPackScriptableObject)itemData).ammoAmount);
        
        private static void Weapon(ServerPlayerManager playerManager, ItemScriptableObject itemData)
        {
        }
    }
}