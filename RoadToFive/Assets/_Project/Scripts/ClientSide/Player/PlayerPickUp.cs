using System;
using System.Collections.Generic;
using _Project.Scripts.ClientSide.LocalPlayer;
using _Project.Scripts.Util.DataStructure;
using _Project.Scripts.Util.Item;
using UnityEngine;

namespace _Project.Scripts.ClientSide.Player
{
    public class PlayerPickUp : MonoBehaviour
    {
        private delegate void PickUpHandler();

        private static readonly Dictionary<int, PickUpHandler> PickUpHandlers = new Dictionary<int, PickUpHandler>()
        {
            {(int)ItemScriptableObject.ItemType.HealthKit, HealthKit},
            {(int)ItemScriptableObject.ItemType.AmmoPack, AmmoPack},
        };

        public void PickUpItem(ItemScriptableObject itemData) => PickUpHandlers[(int)itemData.itemType]();

        private static void HealthKit()
        {
            
        }

        private static void AmmoPack()
        {
            
        }
    }
}