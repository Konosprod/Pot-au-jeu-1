using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IPointerClickHandler {

    public Image image;
    public BodyPart part;
    public Image backgroundColor;

    public Builder builder;

    private bool isSelected = false;
    private Color selectedColor = new Color(.6f,.6f,.6f,1f);
    private Color unselectedColor = new Color(.1f,.1f,.1f,1f);

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void OnPointerClick(PointerEventData eventData)
    {
        isSelected = !isSelected;
        backgroundColor.color = (isSelected == true) ? selectedColor : unselectedColor;

        if(isSelected)
        {
            builder.selectedItem = this.gameObject;
        }
        else
        {
            builder.selectedItem = null;
        }
    }

}
