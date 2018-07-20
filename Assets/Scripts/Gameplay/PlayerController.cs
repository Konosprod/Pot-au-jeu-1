using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    private Rigidbody2D rb;

    private Vector2 speed = new Vector2(2, 2);
    private Vector2 movement;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

        movement = new Vector2(input.x * speed.x, input.y * speed.y);		
	}

    void FixedUpdate()
    {
        rb.velocity = movement;
    }
}
