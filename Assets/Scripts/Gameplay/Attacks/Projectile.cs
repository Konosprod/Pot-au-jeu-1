using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Attack {

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (childCol.IsPlayer())
        {
            if (Input.GetMouseButton(1))
            {
                if (canAttack)
                {
                    UseAttack();
                }
            }
        }
    }
}
