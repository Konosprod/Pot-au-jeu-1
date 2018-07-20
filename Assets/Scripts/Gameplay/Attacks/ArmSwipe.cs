﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Swipe your arms to attack
public class ArmSwipe : MonoBehaviour {

    private ChildCollider childCol;

    public float attackTime = 0.5f;
    public bool canAttack = true;

    public bool forward = true;

	// Use this for initialization
	void Start () {
        childCol = GetComponent<ChildCollider>();
	}

    void Update()
    {
        if (Input.GetKey(KeyCode.T))
        {
            if (canAttack)
            {
                StartCoroutine(Rotate(attackTime));
            }
        }
    }

    IEnumerator Rotate(float duration)
    {
        Quaternion defaultRot = transform.rotation;
        transform.rotation = transform.rotation * Quaternion.AngleAxis(-45f, (forward)?Vector3.forward:Vector3.back);
        Quaternion startRot = transform.rotation;

        childCol.activeAttack = true;
        canAttack = false;
        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            transform.rotation = startRot * Quaternion.AngleAxis(t / duration * 90f, (forward) ? Vector3.forward : Vector3.back); //or transform.right if you want it to be locally based
            yield return null;
        }
        transform.rotation = defaultRot;
        canAttack = true;
        childCol.activeAttack = false;
    }
}