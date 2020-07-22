using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class Inventory : MonoBehaviour
{
    public const int maxInvetory = 10;

    [Serializable]
    public class InventoryItem
    {
        public GameObject item;
        public int count;
        public bool stackable;
        public bool isWeapon;
        int limit;

        public void InitItem()
        {
            item = null;
            count = 0;
            stackable = false;
            isWeapon = false;
            limit = 0;
        }
    }

    [Serializable]
    public class AmmoItem
    {
        public AmmoType ammoType;
        public int count;
    }

    public InventoryItem[] inventory;
    public AmmoItem[] munition;
    public InventoryItem itemInHand;
    public int currentItemInHand;


    public int GetAmmoCount(AmmoType ammoType)
    {
        return munition[(int)ammoType].count;
    }

    void Start()
    {
        inventory = new InventoryItem[maxInvetory];
        currentItemInHand = 0;
        itemInHand = inventory[0];
    }

    void Update()
    {
        if (Keyboard.current.qKey.isPressed)
        {
            DropItem();
        }

        ScrollItems();

        UpdateCurrentWeaponAmmo();
    }

    public void DropItem()
    {
        itemInHand.count--;
        if (itemInHand.count <= 0)
        {
            GameObject.Destroy(itemInHand.item);
            inventory[currentItemInHand].InitItem();
        }
    }
    public void PickUpItem()
    {
        // TODO
    }

    public void ScrollItems()
    {
        Vector2Control wheelInput = Mouse.current.scroll;

        if (wheelInput.y.ReadValue() != 0)
        {
            currentItemInHand += Math.Sign(wheelInput.y.ReadValue());
            if (currentItemInHand < 0)
            {
                currentItemInHand = maxInvetory - 1;
            }
            if (currentItemInHand >= maxInvetory)
            {
                currentItemInHand = 0;
            }
            itemInHand = inventory[currentItemInHand];
        }
    }

    public void UpdateCurrentWeaponAmmo()
    {
        if (itemInHand != null && itemInHand.isWeapon)
        {
            itemInHand.item.GetComponent<WeaponStats>().ammoCount = GetAmmoCount(itemInHand.item.GetComponent<WeaponStats>().ammoType);
        }
    }
}

public enum AmmoType
{
    ammo1 = 0,
    ammo2 = 1,
    ammo3 = 2
    // TODO: Sa ne gandim ce fel de munitie folosim
}
