using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Health : MonoBehaviour {

    public bool isPlayer = false;

    public int hp = 100;
   

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
                Debug.Log("You are dead.");
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    
}
