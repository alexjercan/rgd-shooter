using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityLogic : MonoBehaviour
{
    public int MAX_HEALTH = 100;

    public int health;

    public bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        health = MAX_HEALTH;
    }


    public void TakeDamage(int damage)
    {
        health -= damage;
        if (damage <= 0)
        {
            isDead = true;
            health = 0;
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
}
