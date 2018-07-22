using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public Fader fader;

	// Use this for initialization
	void Start () {
        SoundManager._instance.PlayMusic(SoundType.Home);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayGame()
    {
        if(SettingsManager._instance.gameSettings.firstRun == false)
        {
            fader.StartFadetoScene("Game");
        }
        else
        {
            fader.StartFadetoScene("Game");
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
