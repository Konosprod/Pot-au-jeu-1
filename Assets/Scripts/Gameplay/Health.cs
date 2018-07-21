using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Health : MonoBehaviour {

    public bool isPlayer = false;

    public int hp = 100;

    private int layerLoot;
    private List<BodyPart> bodyParts;
    private Inventory inventory;

    private bool dead = false;

    void Awake()
    {
        layerLoot = LayerMask.NameToLayer("Loot");
        bodyParts = new List<BodyPart>(GetComponentsInChildren<BodyPart>());

        if (isPlayer)
            inventory = GetComponent<Inventory>();
    }

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
            if (!dead) // Prevents monsters from dying multiple times (and dropping more loot thna intended)
            {
                if (isPlayer)
                {
                    Debug.Log("You are dead.");
                    GameManager._instance.LoseTheGame();
                }
                else
                {
                    if (Random.Range(1, 100) < 100)
                    {
                        BodyPart drop = bodyParts[Random.Range(0, bodyParts.Count)];
                        drop.transform.parent = null;
                        drop.GetComponent<Collider2D>().isTrigger = true;
                        drop.gameObject.layer = layerLoot;
                    }
                    Destroy(gameObject);
                }
            }
            dead = true;
        }
    }

    public void PickupLoot(BodyPart drop)
    {
        if(isPlayer)
        {
            inventory.stock[drop.id] += 1;
            Destroy(drop.gameObject);
            
            /*foreach (int id in inventory.stock.Keys)
            {
                Debug.Log("Key : " + id + ", stock : " + inventory.stock[id]);
            }*/
        }
        else
        {
            Debug.LogError("A monster is trying to steal the loot ?!");
        }
    }
}
