using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum SnapType
{
    Head,
    Arm,
    Leg
}

public class Snappoint : MonoBehaviour/*, IPointerEnterHandler, IPointerExitHandler*/ {

    public SnapType type;
    public Color selectedColor;
    public Color unselectedColor;

    public SpriteRenderer spriteRenderer;

    public GameObject body;
    public GameObject part;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /*
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("here");
        spriteRenderer.color = selectedColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        spriteRenderer.color = unselectedColor;
    }
    */

    public void Test()
    {
        Debug.Log("here");
    }
}
