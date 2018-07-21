using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Builder : MonoBehaviour {

    public GameObject listItems;
    public GameObject player;

    public GameObject prefabItem;
    public GameObject prefabText;

    public GameObject selectedItem = null;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        loadItems();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Snappoint"));

            if (hit.collider != null)
            {
                if(selectedItem != null)
                {
                    Snappoint sp = hit.collider.GetComponent<Snappoint>();

                    if(sp != null)
                    {
                        Transform t = sp.part.transform;
                    }
                }
            }
        }
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
            ivItem.builder = this;

            switch(bp.partType)
            {
                case PartType.Arm:
                    {
                        GameObject textRange = Instantiate(prefabText, ivItem.transform);
                        textRange.GetComponent<Text>().text = "Range : " + bp.range;

                        GameObject textDamage = Instantiate(prefabText, ivItem.transform);
                        textDamage.GetComponent<Text>().text = "Damage : " + bp.damages;
                    }
                    break;
            }

            goItem.transform.SetParent(listItems.transform);
        }
    }
}
