using UnityEditor.Animations;
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


    public Vector3 localScale;

    public GameObject owner;
    public AnimatorController animatorController;

    public bool CanShoot()
    {
        return ammoCount != 0;
    }

    public void OnWeaponChange()
    {
        if (owner.GetComponent<Inventory>().weaponPoint.childCount != 0)
        {
            owner.GetComponent<Inventory>().weaponPoint.GetChild(0).gameObject.SetActive(false);
            owner.GetComponent<Inventory>().weaponPoint.GetChild(0).SetParent(owner.transform);
        }
        this.transform.SetParent(owner.GetComponent<Inventory>().weaponPoint);
        this.gameObject.SetActive(true);
        this.transform.localPosition = Vector3.zero;
        this.transform.localRotation = Quaternion.Euler(Vector3.zero);
        this.transform.localScale = localScale;

        owner.GetComponent<Inventory>().weaponPoint.localPosition = spawnPointPosition;
        owner.GetComponent<Inventory>().weaponPoint.localRotation = Quaternion.Euler(spawnPointRotation);
        owner.GetComponent<Inventory>().weaponPoint.localScale = spawnPointScale;

        owner.GetComponent<Inventory>().animatorRig.GetComponent<Animator>().runtimeAnimatorController = animatorController;
    }
}



