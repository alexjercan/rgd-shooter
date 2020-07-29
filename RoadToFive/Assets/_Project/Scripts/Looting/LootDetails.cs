using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootDetails : MonoBehaviour
{
    public GameObject owner;

    public int count;
    public bool stackable;
    
    public bool isWeapon;

    public bool isAmmo;
    public AmmoType ammoType;

    public bool isMedKit;

    public Sprite InventoryPic;
}
