using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkNova : Attack {

    public GameObject inknovaParticle;

    public int damage = 4;

    // Use this for initialization
    void Start()
    {

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

        GameObject newInknova = Instantiate(inknovaParticle, transform.position, Quaternion.identity);

        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 9f);
        foreach(Collider2D col in cols)
        {
            ChildCollider cc = col.gameObject.GetComponent<ChildCollider>();
            if (cc != null && !cc.IsPlayer())
            {
                Debug.Log(col.gameObject.name);
                cc.TakeDamage(damage);
            }
        }

        Destroy(newInknova, 1f);

        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            yield return null;
        }

        canAttack = true;
    }
}
