using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSounds : MonoBehaviour {

    private Button button;
    public SFXType onClickSound;

	// Use this for initialization
	void Start () {
        button = GetComponent<Button>();
        button.onClick.AddListener(PlayClickSound);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void PlayClickSound()
    {
        SoundManager._instance.PlaySFX(onClickSound);
    }
}
