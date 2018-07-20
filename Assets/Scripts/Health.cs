using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public bool isPlayer = false;

    private int hp = 100;
   

    public void SetHealth(int health)
    {
        hp = health;
    }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        if(hp <= 0)
        {
            hp = 0;
            if(isPlayer)
            {

            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    
}
