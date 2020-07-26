using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    public int ammoCount;
    public int rateOfFire;
    public int damage;
    public AmmoType ammoType;

    public Vector3 spawnPointPosition;
    public Vector3 spawnPointRotation;
    public Vector3 spawnPointScale;

    public GameObject owner;

    public bool CanShoot()
    {
        return ammoCount != 0;
    }
}



