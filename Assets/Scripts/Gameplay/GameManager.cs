using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager _instance;

    private bool isOver = false;

	// Use this for initialization
	void Awake () {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
	}

    // The player died RIP
    public void LoseTheGame()
    {
        isOver = true;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
