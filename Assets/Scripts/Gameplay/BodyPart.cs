using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PartType
{
    Arm,
    Leg,
    Body,
    Head
}

public enum PartOrientation
{
    Left,
    Right
}

public class BodyPart : MonoBehaviour {

    public PartType partType;
    public bool canPick;
    public Sprite itemSprite;

    [Header("Leg Values")]
    //Leg Values
    public float stamina;
    public float movementSpeed;
    public float sprintSpeed;
    public PartOrientation legOrientation;

    [Header("Arm Values")]
    //Arm Values
    public float damages;
    public float range = 1f;
    public PartOrientation armOrientation;


    [Header("Body Values")]
    //Body Values
    public float health;
    public float armor;


    [Header("Head Values")]
    //Head Values
    public float intelligence;
    public Skill skill;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void Animate()
    {

    }
}
