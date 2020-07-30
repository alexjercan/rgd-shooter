using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityLogic : MonoBehaviour
{
    public int MAX_HEALTH = 100;
    public int MAX_ARMOR = 100;

    public int health;
    public int armor;

    public bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        health = MAX_HEALTH;
    }


    public void TakeDamage(int damage)
    {
        if (armor > 0)
        {
            armor -= damage;
            if (armor < 0)
            {
                health += armor;
                armor = 0;
            }
        }
        else
        {
            health -= damage;
            if (health <= 0)
            {
                isDead = true;
                health = 0;
            }
        }
    }

    public void TakeHeal(int heal)
    {
        health += heal;
        if (health > MAX_HEALTH)
        {
            health = MAX_HEALTH;
        }
    }

    public void TakeArmor(int armorGained)
    {
        armor += armorGained;
        if (armor > MAX_ARMOR)
        {
            armor = MAX_ARMOR;
        }
    }
}
