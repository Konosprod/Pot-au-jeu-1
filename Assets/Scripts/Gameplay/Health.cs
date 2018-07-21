using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class Health : MonoBehaviour {

    public bool isPlayer = false;

    public float maxHp = 100;
    public float hp = 100;

    public float maxShield = 100;
    public float shield = 0;

    public Image healthBar;
    public Image shieldBar;

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

    private void UpdateShieldBar()
    {
        if (shieldBar)
        {
            if (shield > 0)
            {
                shieldBar.transform.parent.gameObject.SetActive(true);
                shieldBar.fillAmount = shield / maxShield;
            }
            else
            {
                shieldBar.transform.parent.gameObject.SetActive(false);
            }
        }
    }

    private void UpdateHealthBar()
    {
        healthBar.fillAmount = hp / maxHp;
    }

    public void AddShield(int shield)
    {
        if (this.shield + shield <= maxShield)
        {
            if (shield == 0)
                shieldBar.transform.parent.gameObject.SetActive(false);

            this.shield += shield;
        }
        else
            this.shield = maxShield;

        UpdateShieldBar();
    }

    public void TakeDamage(int dmg)
    {

        if (shield - dmg < 0)
        {
            float left = dmg - shield;
            shield = 0;
            hp -= left;
        }
        else
        {
            shield -= dmg;
        }


        if (hp <= 0)
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

        if (isPlayer)
        {
            UpdateShieldBar();
            UpdateHealthBar();
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
