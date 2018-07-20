using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Builder : MonoBehaviour {

    public GameObject listItems;
    public GameObject player;

    public GameObject prefabItem;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        loadItems();
    }

    private void loadItems()
    {
        Inventory inventory = player.GetComponent<Inventory>();

        foreach (BodyPart bp in inventory.items)
        {
            GameObject goItem = Instantiate(prefabItem.gameObject);

            InventoryItem ivItem = goItem.GetComponent<InventoryItem>();

            ivItem.part = bp;
            ivItem.image.sprite = bp.itemSprite;

            Text text = ivItem.gameObject.AddComponent<Text>();

            text.color = Color.white;
            text.text = "LOL TEST";

            goItem.transform.SetParent(listItems.transform);
        }
    }
}
