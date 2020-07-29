using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class Inventory : MonoBehaviour
{
    /*
     * IMPORTANT!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
     * La multiplayer pot aparea desincronizari din cauza Destroy-ului
     */



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

    float dropTimer;

    public Transform weaponPoint;
    public GameObject animatorRig;

    private InventoryItem bestHeal;

    public int GetAmmoCount(AmmoType ammoType)
    {
        return munition[(int)ammoType].count;
    }

    void Start()
    {
        currentItemInHand = 0;
        itemInHand = inventory[0];
        dropTimer = 1 / 20.0f;  
    }

    void Update()
    {
        if (Keyboard.current.qKey.isPressed)
        {
            DropItem();
        }

        ScrollItems();

        UpdateCurrentWeaponAmmo();

        HealPlayer();
    }

    public void DropItem()
    {
        dropTimer += Time.deltaTime;
        if (dropTimer >= 1 / 20.0f)
        {
            itemInHand.count--;
            if (itemInHand.count <= 0)
            {
                Destroy(itemInHand.item);
                inventory[currentItemInHand].InitItem();
            }
            dropTimer = 0;
        }
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
            if (itemInHand.item != null)
            {
                if (itemInHand.item.GetComponent<LootDetails>().isWeapon)
                {
                    itemInHand.item.GetComponent<WeaponStats>().OnWeaponChange();
                }
            }
        }
    }

    public void UpdateCurrentWeaponAmmo()
    {
        if (itemInHand != null && itemInHand.isWeapon)
        {
            itemInHand.item.GetComponent<WeaponStats>().ammoCount = GetAmmoCount(itemInHand.item.GetComponent<WeaponStats>().ammoType);
        }
    }

    public bool hasItem(GameObject item)
    {
        foreach (InventoryItem invItem in inventory)
        {
            if (invItem.item != null && invItem.item.name == item.name)
            {
                return true;
            }
        }
        return false;
    }

    public int itemIndex(GameObject item)
    {
        int idx = 0;
        foreach (InventoryItem invItem in inventory)
        {
            if (invItem.item != null && invItem.item.name == item.name)
            {
                return idx;
            }
            idx++;
        }
        return -1;
    }

    public int freeIndex()
    {
        int idx = 0;
        foreach(InventoryItem invItem in inventory)
        {
            if (invItem.item == null)
            {
                return idx;
            }
            idx++;
        }
        return -1;
    }

    public void AddItem(GameObject item)
    {
        if (hasItem(item))
        {
            if (item.GetComponent<LootDetails>().stackable)
            {
                inventory[itemIndex(item)].count += item.GetComponent<LootDetails>().count;
                Destroy(item);
            } else
            {
                Debug.Log("You already have this item");
            }
        } else
        {
            int idx = freeIndex();
            if (idx != -1) // not full
            {
                item.GetComponent<LootDetails>().owner = this.gameObject;
                if (item.GetComponent<LootDetails>().isWeapon)
                {
                    item.GetComponent<WeaponStats>().owner = this.gameObject;
                }

                inventory[idx].item = item;
                inventory[idx].stackable = item.GetComponent<LootDetails>().stackable;
                inventory[idx].count = item.GetComponent<LootDetails>().count;
                inventory[idx].isWeapon = item.GetComponent<LootDetails>().isWeapon;

                item.transform.SetParent(this.gameObject.transform);
                item.transform.localPosition = Vector3.zero;
                item.SetActive(false);
            } else
            {
                Debug.Log("Inventory is full");
            }
        }
    }

    public void AddAmmo(AmmoType ammoType, int count)
    {
        munition[(int)ammoType].count += count;
    }

    private InventoryItem GetBestSuitableHeal()
    {
        int healthHandicap = GetComponent<EntityLogic>().MAX_HEALTH - GetComponent<EntityLogic>().health;

        InventoryItem bestSuitableHeal = null;

        foreach(InventoryItem itemInv in inventory)
        {
            if (itemInv.item != null && itemInv.item.GetComponent<LootDetails>().isMedKit)
            {
                if (bestSuitableHeal == null)
                {
                    bestSuitableHeal = itemInv;
                } else
                {
                    if (bestSuitableHeal.item.GetComponent<MedKitLogic>().amount < itemInv.item.GetComponent<MedKitLogic>().amount)
                    {
                        bestSuitableHeal = itemInv;
                    }
                }
            }
        }

        return bestSuitableHeal;
    }

    public void HealPlayer()
    {
        if (bestHeal != null)
        {
            if (bestHeal.item.GetComponent<MedKitLogic>().isHealing)
            {
                bestHeal.item.GetComponent<MedKitLogic>().UseMedKit();
            }
            else
            {
                bestHeal = null;
            }
        }

        if (Keyboard.current.hKey.wasPressedThisFrame && GetComponent<EntityLogic>().health < GetComponent<EntityLogic>().MAX_HEALTH)
        {
            bestHeal = GetBestSuitableHeal();
            if (bestHeal != null)
            {
                bestHeal.item.GetComponent<MedKitLogic>().isHealing = true;
            }
        }
    }
}

public enum AmmoType
{
    NONE = 99,
    CINCI = 0,
    SASE = 1,
    SAPTE = 2,
    OPT = 3,
    NOUA = 4,
    ZECE = 5
    // TODO: Sa ne gandim ce fel de munitie folosim
}
