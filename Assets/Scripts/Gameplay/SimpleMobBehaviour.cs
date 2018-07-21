using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMobBehaviour : MonoBehaviour {

    public float speed = 2.3f;
    private GameObject player;

    private List<Attack> attacks;

    private float distanceToPlayer;

	// Use this for initialization
	void Awake ()
    {
		attacks = new List<Attack>(GetComponentsInChildren<Attack>());
	}
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

	
	// Update is called once per frame
	void Update () {
        if(player != null)
        {
            distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);


            // Move closer to the player
            if(distanceToPlayer > 4f)
            {
                Vector3 move = ((player.transform.position - transform.position).normalized);
                move = move * speed * Time.deltaTime;
                //Debug.Log("Move : x=" + move.x + ", y=" + move.y + ", z=" + move.z);
                transform.position += move;
            }

            // Use your attacks
            foreach (Attack atk in attacks)
            {
                if(distanceToPlayer < 7f && atk.canAttack)
                {
                    if(Random.Range(1, 100) < 5)
                    {
                        atk.UseAttack();
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Player not found");
        }
	}
}
