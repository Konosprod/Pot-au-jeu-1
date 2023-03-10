using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCollider : MonoBehaviour {

    [HideInInspector]
    public bool activeAttack = false;

    public int atk = 10;

    private Health parentHP;
    private int layerLoot;
    private int layerMapTrigger;
    private int layerChestTrigger;
    private int layerProjectile;


    // Use this for initialization
    void Awake ()
    {
        layerLoot = LayerMask.NameToLayer("Loot");
        layerMapTrigger = LayerMask.NameToLayer("MapTrigger");
        layerChestTrigger = LayerMask.NameToLayer("ChestTrigger");
        layerProjectile = LayerMask.NameToLayer("Projectile");
        parentHP = transform.parent.gameObject.GetComponent<Health>();
        if(parentHP == null)
            parentHP = GetComponentInParent<Health>();
    }

    public void TakeDamage(int dmg)
    {
        parentHP.TakeDamage(dmg);
    }

    public bool IsPlayer()
    {
        return parentHP.isPlayer;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //Debug.Log("OnCollisionEnter2D");
        ChildCollider otherCol = col.collider.gameObject.GetComponent<ChildCollider>();

        if (otherCol != null && (IsPlayer() || otherCol.IsPlayer()))
        {
            //Debug.Log("Time to hit : self.activeAtk = " + activeAttack + ", other.activeAtk = " + otherCol.activeAttack);
            // Deal damage to the other if we are active
            if (activeAttack)
            {
                otherCol.TakeDamage(atk);
            }
        }
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if(IsPlayer() && col.gameObject.layer == layerLoot)
        {
            BodyPart drop = col.gameObject.GetComponent<BodyPart>();
            if (drop != null)
            {
                parentHP.PickupLoot(drop);
            }
            else
            {
                Debug.LogError("The drop has no bodypart script : " + col.gameObject.name);
            }
        }

        if(IsPlayer() && col.gameObject.layer == layerMapTrigger)
        {
            GameManager._instance.ShowNextMaps();
        }

        if(IsPlayer() && col.gameObject.layer == layerChestTrigger)
        {
            parentHP.GetRareLoot();
        }

        if(!IsPlayer() && col.gameObject.layer == layerProjectile)
        {
            parentHP.TakeDamage(col.gameObject.GetComponent<ProjectileMove>().damage);
            Destroy(col.gameObject);
        }
    }
}
