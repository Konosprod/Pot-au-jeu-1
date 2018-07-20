using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    private Rigidbody2D rb;

    private Vector2 speed = new Vector2(10, 10);
    private Vector2 movement;

    private int layerLoot;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        layerLoot = LayerMask.NameToLayer("Loot");
	}
	
	// Update is called once per frame
	void Update () {

	}

    void FixedUpdate()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        
        rb.velocity = new Vector2(input.x * speed.x, input.y * speed.y);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.layer == layerLoot)
        {

        }
    }
}
