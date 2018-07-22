using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vomit : Attack {

    public GameObject vomitParticle;

    public int damage = 4;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
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
        
        Vector2 cursorInWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = cursorInWorldPos - new Vector2(transform.position.x, transform.position.y);
        GameObject newVomit = Instantiate(vomitParticle, transform.position, Quaternion.Euler(-90,0,0) * Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.up, dir), Vector2.down), transform);

        RaycastHit2D[] rayHits = Physics2D.CircleCastAll(transform.position, 2f, dir, 5f);
        foreach (RaycastHit2D rayHit in rayHits)
        {
            ChildCollider cc = rayHit.collider.gameObject.GetComponent<ChildCollider>();
            if (cc != null && !cc.IsPlayer())
            {
                Debug.Log(rayHit.collider.gameObject.name);
                cc.TakeDamage(damage);
            }
        }

        Destroy(newVomit, 1f);

        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            yield return null;
        }

        canAttack = true;
    }
}
