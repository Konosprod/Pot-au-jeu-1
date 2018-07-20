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
    public Sprite sprite;

    //Leg Values
    public float stamina;
    public float movementSpeed;
    public float sprintSpeed;
    public PartOrientation legOrientation;

    //Arm Values
    public float damages;
    public float range;
    public PartOrientation armOrientation;

    //Body Values
    public float health;
    public float armor;

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
