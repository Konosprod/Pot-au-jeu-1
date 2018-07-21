using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Builder : MonoBehaviour {

    public GameObject listItems;
    public GameObject player;

    public GameObject prefabItem;
    public GameObject prefabText;

    public int selectedItem = 0;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Inventory>().stock.Add(7, 2);
        loadItems();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Snappoint"));

            if (hit.collider != null)
            {
                if(selectedItem > 0)
                {
                    Snappoint sp = hit.collider.GetComponent<Snappoint>();

                    if(sp != null)
                    {
                        Transform t = sp.part.transform;
                        Inventory inventory = player.GetComponent<Inventory>();

                        for(int i = 0; i < inventory.items.Length; i++)
                        {
                            if(inventory.items[i].GetComponent<BodyPart>().id == selectedItem)
                            {
                                GameObject part = Instantiate(inventory.items[i].gameObject, player.transform.GetChild(0).transform);
                                Destroy(sp.part.gameObject);
                                sp.part = part;
                                inventory.stock[selectedItem]--;
                                loadItems();
                            }
                        }

                    }
                }
            }
        }
    }

    private void CleanItems()
    {
        for(int i = 0; i < listItems.transform.childCount; i++)
        {
            Destroy(listItems.transform.GetChild(i));
        }
    }

    private void loadItems()
    {
        CleanItems();
        Inventory inventory = player.GetComponent<Inventory>();

        foreach (GameObject go in inventory.items)
        {
            BodyPart bp = go.GetComponent<BodyPart>();

            if (inventory.stock.ContainsKey(bp.id))
            {

                if (inventory.stock[bp.id] > 0)
                {
                    GameObject goItem = Instantiate(prefabItem.gameObject);

                    InventoryItem ivItem = goItem.GetComponent<InventoryItem>();

                    ivItem.image.sprite = bp.itemSprite;
                    ivItem.id = bp.id;
                    ivItem.builder = this;

                    switch (bp.partType)
                    {
                        case PartType.Arm:
                            {
                                GameObject textRange = Instantiate(prefabText, ivItem.transform);
                                textRange.GetComponent<Text>().text = "Range : " + bp.range;

                                GameObject textDamage = Instantiate(prefabText, ivItem.transform);
                                textDamage.GetComponent<Text>().text = "Damage : " + bp.damages;
                            }
                            break;

                        case PartType.Leg:
                            {

                            }
                            break;

                        case PartType.Body:
                            {

                            }
                            break;

                        case PartType.Head:
                            {

                            }
                            break;
                    }

                    GameObject textQty = Instantiate(prefabText, ivItem.transform);
                    textQty.GetComponent<Text>().text = "Qty : " + inventory.stock[bp.id];

                    goItem.transform.SetParent(listItems.transform);
                }
            }
        }
    }
}
