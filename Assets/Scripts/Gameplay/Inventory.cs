using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public GameObject[] items;
    public Dictionary<int, int> stock = new Dictionary<int, int>();

	// Use this for initialization
	void Awake () {
        foreach(GameObject item in items)
        {
            BodyPart bp = item.GetComponent<BodyPart>();
            stock.Add(bp.id, 0);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
