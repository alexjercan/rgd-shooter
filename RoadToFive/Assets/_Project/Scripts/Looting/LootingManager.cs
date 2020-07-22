using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

// Acest script trebuie pus pe main camera al playerului
// Prin apasarea tastei E poti lua obiecte apropiate de pe jos
// Obiectele pe care le poti lua de pe jos trebuie sa 
// - aiba tag-ul "LootItem"
// - aiba Loot Details (Script) adaugat
// Armele de asemenea trebuie sa aiba adaugate Scriptul Weapon Stats
// !! Contine erori de multiplayer cauzate de destroy (?)
public class LootingManager : MonoBehaviour
{
    string itemTag = "LootItem";
    float rayDistance = 10.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, rayDistance)) {
            if (hit.collider.tag == itemTag) {
                if (Keyboard.current.eKey.wasPressedThisFrame)
                {
                    pickUpItem(hit);
                }
            }
        }
    }

    void pickUpItem(RaycastHit hit)
    {
        
        
        GameObject item = hit.transform.gameObject;
        if (item.GetComponent<LootDetails>().isAmmo == true)
        {
            this.transform.parent.GetComponent<Inventory>().
                AddAmmo(item.GetComponent<LootDetails>().ammoType,
                    item.GetComponent<LootDetails>().count);
            
            // !!! La Multi Player s-ar putea sa intervina erori aici
            Destroy(hit.transform.gameObject);
        } else
        {
            this.transform.parent.GetComponent<Inventory>().
            AddItem(item);
            UnityEngine.Debug.Log("Item Picked");
        }
    }
}
