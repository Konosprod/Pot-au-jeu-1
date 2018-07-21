using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCollider : MonoBehaviour {

    public bool activeAttack = false;

    public int atk = 10;

    private Health parentHP;
    private int layerLoot;


    // Use this for initialization
    void Awake ()
    {
        layerLoot = LayerMask.NameToLayer("Loot");
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
    }
}
