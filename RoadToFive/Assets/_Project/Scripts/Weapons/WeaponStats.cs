using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    public int ammoCount;
    public int rateOfFire;
    public int damage;
    public AmmoType ammoType;

    public GameObject owner;

    public bool CanShoot()
    {
        return ammoCount != 0;
    }
}



