using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonChoice : MonoBehaviour {

    public int id;

	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(ChooseThisRoom);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChooseThisRoom()
    {
        GameManager._instance.currentRoomId = id;
        GameManager._instance.LoadNextScene();
    }
}
