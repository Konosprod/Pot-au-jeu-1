using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{

    public float fadeTime;
    public Image toFade;

    public GameObject[] slides;

    private bool fadeOut = false;
    private bool fadeIn = false;

    private int currentSlide = 0;
    private float elaspedTime;

    public Image ownFade;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartFadetoScene(string name)
    {
        StartCoroutine(FadeToScene(name));
    }

    public void StartIntro()
    {
        StartCoroutine(fadeIntro());
    }

    private IEnumerator FadeToScene(string sceneName)
    {
        elaspedTime = 0;
        ownFade.gameObject.SetActive(true);

        while (elaspedTime < fadeTime)
        {
            /*
                float percentage = elaspedTime / fadeTime;
                ownFade.color = new Color(toFade.color.r, toFade.color.g, toFade.color.b, Mathf.Lerp(1, 0, percentage));
            */
            float percentage = elaspedTime / fadeTime;
            ownFade.color = new Color(ownFade.color.r, ownFade.color.g, ownFade.color.b, Mathf.Lerp(0, 01, percentage));


            elaspedTime += Time.deltaTime;

            yield return null;
        }

        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
    }

    private IEnumerator fadeIntro()
    {
        while (currentSlide < slides.Length)
        {
            if (elaspedTime >= fadeTime)
            {
                if (!fadeOut && !fadeIn)
                {
                    fadeOut = true;
                }
                else if (fadeOut && !fadeIn)
                {
                    fadeIn = true;
                }
                elaspedTime = 0;
            }
            else
            {
                elaspedTime += Time.deltaTime;
            }

            if (!fadeOut)
            {
                float percentage = elaspedTime / fadeTime;
                //Debug.Log(Mathf.Lerp(1, 0, percentage));
                toFade.color = new Color(toFade.color.r, toFade.color.g, toFade.color.b, Mathf.Lerp(1, 0, percentage));
            }
            else if (!fadeIn)
            {
                float percentage = elaspedTime / fadeTime;
                toFade.color = new Color(toFade.color.r, toFade.color.g, toFade.color.b, Mathf.Lerp(0, 01, percentage));
            }

            if (fadeIn && fadeOut)
            {
                fadeIn = false;
                fadeOut = false;
                elaspedTime = 0;
                slides[currentSlide].gameObject.SetActive(false);
                currentSlide++;
            }

            yield return null;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
