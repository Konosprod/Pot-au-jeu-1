using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Attack {

    public GameObject projectile;

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

    public override void UseAttack()
    {
        if (childCol.IsPlayer())
        {
            StartCoroutine(Shoot(attackTime));
        }
    }

    IEnumerator Shoot(float duration)
    {
        canAttack = false;

        GameObject newProj = Instantiate(projectile, transform.position, Quaternion.identity);
        Vector2 cursorInWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = cursorInWorldPos - new Vector2(transform.position.x, transform.position.y);
        // Debug.Log("Mouse position : " + cursorInWorldPos + ", Direction : " + dir);
        newProj.GetComponent<ProjectileMove>().movement = dir.normalized * 12f;

        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            yield return null;
        }

        canAttack = true;
    }
}
