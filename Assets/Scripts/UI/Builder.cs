using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour {

    public GameObject listItems;
    public GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        loadItems();
    }

    private void loadItems()
    {
        Inventory inventory = player.GetComponent<Inventory>();
    }
}
