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
        player.GetComponent<Inventory>().stock[1] = 2;
        player.GetComponent<Inventory>().stock[2] = 2;
        player.GetComponent<Inventory>().stock[3] = 2;
        player.GetComponent<Inventory>().stock[4] = 2;
        player.GetComponent<Inventory>().stock[5] = 2;
        player.GetComponent<Inventory>().stock[6] = 2;
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
                        Inventory inventory = player.GetComponent<Inventory>();

                        for(int i = 0; i < inventory.items.Length; i++)
                        {
                            if(inventory.items[i].GetComponent<BodyPart>().id == selectedItem)
                            {
                                //Init
                                GameObject part = Instantiate(inventory.items[i].gameObject, sp.transform.parent.transform);
                                BodyPart bp = part.GetComponent<BodyPart>();

                                switch(sp.type)
                                {
                                    case SnapType.Head:
                                        {
                                            if(bp.partType == PartType.Arm)
                                            {
                                                part.transform.localEulerAngles = new Vector3(0, 0, 180);
                                            }

                                            if(bp.partType == PartType.Leg)
                                            {
                                                part.transform.localEulerAngles = new Vector3(0, 0, 180);
                                            }
                                        }
                                        break;

                                    case SnapType.Arm:
                                        {
                                            if(bp.partType == PartType.Head)
                                            {
                                                part.transform.localEulerAngles = new Vector3(0, 0, -90);
                                            }
                                            if(bp.partType == PartType.Arm)
                                            {
                                                part.transform.localEulerAngles = sp.transform.localEulerAngles;
                                            }
                                            if(bp.partType == PartType.Leg && bp.legOrientation == PartOrientation.Left)
                                            {
                                                part.transform.localEulerAngles = sp.transform.localEulerAngles;
                                                //part.transform.localEulerAngles = new Vector3(0, 0, 90);
                                            }
                                        }
                                        break;

                                    case SnapType.Leg:
                                        {
                                            if(bp.partType == PartType.Head)
                                            {
                                                part.transform.localEulerAngles = new Vector3(0, 0, 180);
                                            }

                                            if(bp.partType == PartType.Arm && bp.armOrientation == PartOrientation.Right)
                                            {
                                                part.transform.localEulerAngles = new Vector3(0, 0, 0);
                                            }
                                        }
                                        break;

                                }

                                //Set Position
                                //part.transform.localRotation = t.localRotation;
                                part.transform.position = sp.transform.position;

                                //Set the good flip
                                part.GetComponent<SpriteRenderer>().flipX = sp.spriteRenderer.flipX;
                                part.GetComponent<SpriteRenderer>().flipY = sp.spriteRenderer.flipY;


                                //Set out
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

    public void ExitToGame()
    {
        GameManager._instance.ShowNextMaps();
    }

    private void CleanItems()
    {
        foreach (Transform child in listItems.transform)
        {
            GameObject.Destroy(child.gameObject);
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
                    ivItem.itemName.text = bp.itemName;

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
                                GameObject textRange = Instantiate(prefabText, ivItem.transform);
                                textRange.GetComponent<Text>().text = "Stamina : " + bp.stamina;

                                GameObject textDamage = Instantiate(prefabText, ivItem.transform);
                                textDamage.GetComponent<Text>().text = "Movement Speed : " + bp.movementSpeed;

                                textDamage = Instantiate(prefabText, ivItem.transform);
                                textDamage.GetComponent<Text>().text = "Sprint Speed : " + bp.sprintSpeed;
                            }
                            break;

                        case PartType.Body:
                            {
                                GameObject textRange = Instantiate(prefabText, ivItem.transform);
                                textRange.GetComponent<Text>().text = "HP : " + bp.health;

                                GameObject textDamage = Instantiate(prefabText, ivItem.transform);
                                textDamage.GetComponent<Text>().text = "Armor : " + bp.armor;
                            }
                            break;

                        case PartType.Head:
                            {
                                GameObject textRange = Instantiate(prefabText, ivItem.transform);
                                textRange.GetComponent<Text>().text = "Intel : " + bp.intelligence;

                                GameObject textDamage = Instantiate(prefabText, ivItem.transform);
                                //textDamage.GetComponent<Text>().text = "Skill : " + bp.skill.skillName;
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
