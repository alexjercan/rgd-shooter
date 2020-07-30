using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    /*
     * IMPORTANT!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
     * La multiplayer pot aparea desincronizari din cauza Destroy-ului
     */



    public const int maxInvetory = 10;
    private Color selected = new Color(0.6367924f, 0.9803672f, 1f, 0.7215686f);
    private Color empty = new Color(1, 0.9932215f, 1, 1);

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

    private InventoryItem healingUsed;
    private InventoryItem armorUsed;

    public GameObject[] InventorySlots;
    public GameObject[] InventoryPics;
    public GameObject[] Counts;

    public int GetAmmoCount(AmmoType ammoType)
    {
        return munition[(int)ammoType].count;
    }

    void Start()
    {
        currentItemInHand = 0;
        itemInHand = inventory[0];
        dropTimer = 1 / 20.0f;

        InventoryPics[0].GetComponent<Image>().sprite = inventory[0].item.GetComponent<LootDetails>().InventoryPic;
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

        updateInventoryGUI();
    }

    private void updateInventoryGUI()
    {
        int index = 0;
        foreach (InventoryItem invItem in inventory)
        {
            if (invItem.item != null)
            {
                if (invItem.isWeapon)
                {
                    Counts[index].GetComponent<Text>().text = munition[(int)invItem.item.GetComponent<WeaponStats>().ammoType].count.ToString();

                }
                else {
                    Counts[index].GetComponent<Text>().text = invItem.count.ToString();
                }
            } else
            {
                Counts[index].GetComponent<Text>().text = "";
            }

            index++;
        }
    }


    public void DropItem()
    {
        if (currentItemInHand != 0)
        {

            dropTimer += Time.deltaTime;
            if (dropTimer >= 1 / 20.0f)
            {
                itemInHand.count--;
                if (itemInHand.count <= 0)
                {
                    bool isWeapon = itemInHand.item.GetComponent<LootDetails>().isWeapon;
                    Destroy(itemInHand.item);
                    inventory[currentItemInHand].InitItem();

                    InventoryPics[currentItemInHand].GetComponent<Image>().sprite = null;

                    if (isWeapon == true)
                    {
                        InventorySlots[currentItemInHand].GetComponent<Image>().color = empty;

                        currentItemInHand = 0;
                        itemInHand = inventory[0];
                        inventory[0].item.GetComponent<WeaponStats>().OnWeaponChange();
                        InventorySlots[currentItemInHand].GetComponent<Image>().color = selected;
                    }
                }
                dropTimer = 0;
            }
        }
    }

    private int FirstOcuppiedSlot(int direction)
    {

        int r = currentItemInHand;
        do
        {
            r += direction;

            if (r < 0)
            {
                r = maxInvetory - 1;
            }
            if (r >= maxInvetory)
            {
                r = 0;
            }
        } while (inventory[r].item == null);

        return r;
    }

    public void ScrollItems()
    {
        Vector2Control wheelInput = Mouse.current.scroll;

        if (wheelInput.y.ReadValue() != 0)
        {

            InventorySlots[currentItemInHand].GetComponent<Image>().color = empty;

            currentItemInHand = FirstOcuppiedSlot(Math.Sign(wheelInput.y.ReadValue()));
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

            InventorySlots[currentItemInHand].GetComponent<Image>().color = selected;

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

                InventoryPics[idx].GetComponent<Image>().sprite = item.GetComponent<LootDetails>().InventoryPic;

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

    public void HealPlayer()
    {
        if (healingUsed != null)
        {
            if (healingUsed.item.GetComponent<MedKitLogic>().isHealing)
            {
                healingUsed.item.GetComponent<MedKitLogic>().UseMedKit();
            }
            else
            {
                healingUsed = null;
            }
        } else
        {
            if (armorUsed != null)
            {
                if (armorUsed.item.GetComponent<ArmorLogic>().isTakingArmor)
                {
                    armorUsed.item.GetComponent<ArmorLogic>().UseArmor();
                }
                else
                {
                    armorUsed = null;
                }
            }
        }

        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            healingUsed = itemInHand.item.GetComponent<LootDetails>().isMedKit ? itemInHand : null;
            if (healingUsed != null)
            {
                healingUsed.item.GetComponent<MedKitLogic>().isHealing = true;
            }
            else
            {
                armorUsed = itemInHand.item.GetComponent<LootDetails>().isArmor ? itemInHand : null;
                if (armorUsed != null)
                {
                    armorUsed.item.GetComponent<ArmorLogic>().isTakingArmor = true;
                }
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
}
