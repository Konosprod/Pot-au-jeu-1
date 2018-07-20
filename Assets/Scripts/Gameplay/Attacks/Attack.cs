using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    [HideInInspector]
    public ChildCollider childCol;

    public float attackTime = 0.5f;
    public bool canAttack = true;

    // Use this for initialization
    void Awake ()
    {
        childCol = GetComponent<ChildCollider>();
    }

    public virtual void UseAttack() { }
	
	// Update is called once per frame
	void Update () {
		
	}
}
