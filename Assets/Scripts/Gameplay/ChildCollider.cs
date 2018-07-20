using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCollider : MonoBehaviour {

    public bool activeAttack = false;

    public int atk = 10;

    private Health parentHP;
        

	// Use this for initialization
	void Start () {
        parentHP = transform.parent.gameObject.GetComponent<Health>();
	}

    public void TakeDamage(int dmg)
    {
        parentHP.TakeDamage(dmg);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        ChildCollider otherCol = col.gameObject.GetComponent<ChildCollider>();

        if (otherCol != null)
        {
            // Deal damage to the other if we are active
            if (activeAttack)
            {
                otherCol.TakeDamage(atk);
            }

            // Take damage from the other if it is active
            if (otherCol.activeAttack)
            {
                otherCol.TakeDamage(atk);
            }
        }
    }
}
