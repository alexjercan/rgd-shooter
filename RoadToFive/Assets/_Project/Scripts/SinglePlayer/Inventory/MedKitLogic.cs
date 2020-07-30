using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class MedKitLogic : MonoBehaviour
{
    const float MAX_TIME_MEDKIT_USE = 5;
    public int amount;
    public float useTime = MAX_TIME_MEDKIT_USE;
    public bool isHealing;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UseMedKit()
    {
        // dureaza 5 sec 
        // daca primeste input se intrerupe + reset time
        // creste hp cu "amount" dupa ce s a terminat "useTime"
        // medKit dispare (count-- / distroy daca 1 singur)
        useTime-= Time.deltaTime;

        if (Keyboard.current.anyKey.wasPressedThisFrame || mouseButtonPressed())
        {
            useTime = MAX_TIME_MEDKIT_USE;
            isHealing = false;
        } else
        {
            if (useTime <= 0)
            {
                useTime = MAX_TIME_MEDKIT_USE;
                isHealing = false;

                GetComponent<LootDetails>().owner.GetComponent<EntityLogic>().TakeHeal(amount);
                Inventory inventory = GetComponent<LootDetails>().owner.GetComponent<Inventory>();
                inventory.inventory[inventory.itemIndex(this.gameObject)].count--;
                if (inventory.inventory[inventory.itemIndex(this.gameObject)].count <= 0)
                {
                    inventory.inventory[inventory.itemIndex(this.gameObject)].InitItem();
                    
                    // TODO: (?) Multi-Player Safe ?
                    Destroy(this.gameObject);
                }
            }
        }
    }

    bool mouseButtonPressed()
    {
        Mouse mouse = Mouse.current;
        if (mouse.backButton.wasPressedThisFrame || mouse.forwardButton.wasPressedThisFrame ||
            mouse.leftButton.wasPressedThisFrame || mouse.middleButton.wasPressedThisFrame ||
            mouse.rightButton.wasPressedThisFrame || mouse.scroll.IsPressed() )
        {
            return true;
        } else
        {
            return false;
        }
    }

}
