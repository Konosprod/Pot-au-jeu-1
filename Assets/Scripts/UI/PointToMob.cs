using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointToMob : MonoBehaviour {

    Image[] imgs;
    Vector3 screenPos;
    Vector2 onScreenPos;

    public GameObject target;

    void Start()
    {
        imgs = GetComponentsInChildren<Image>();
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        screenPos = Camera.main.WorldToViewportPoint(target.transform.position); //get viewport positions

        if (screenPos.x >= 0 && screenPos.x <= 1 && screenPos.y >= 0 && screenPos.y <= 1)
        {
            foreach (Image img in imgs)
            {
                img.enabled = false;
            }
            return;
        }
        else
        {
            foreach (Image img in imgs)
            {
                img.enabled = true;
            }
        }

        onScreenPos = new Vector2(screenPos.x - 0.5f, screenPos.y - 0.5f) * 2; //2D version, new mapping
        onScreenPos = (onScreenPos / 2) + new Vector2(0.5f, 0.5f); //undo mapping
        onScreenPos = new Vector2(Mathf.Clamp(onScreenPos.x, 0.03f, 0.97f), Mathf.Clamp(onScreenPos.y, 0.05f, 1f));
        if (onScreenPos.x < 0.5f)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
        else
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        //Debug.Log(onScreenPos);
        transform.position = Camera.main.ViewportToScreenPoint(onScreenPos);
    }
}
