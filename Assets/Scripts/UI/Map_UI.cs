using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Map_UI : MonoBehaviour
{
    public RectTransform rectTransform;
    public LaKiaroInfo_UI lakiaroInfo;
    Vector3 oldPos, newPos;
    [SerializeField]
    float speed = 1f;

    public void OnBeginDrag()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            oldPos = Input.GetTouch(0).position;
        }
        else
        {
            oldPos = Input.mousePosition;
        }
    }

    public void OnDrag()
    {

        if (Application.platform == RuntimePlatform.Android)
        {
            newPos = Input.GetTouch(0).position;
        }
        else
        {
            newPos = Input.mousePosition;
        }

        Vector3 cPos = newPos - oldPos;
        cPos.y = 0;
        cPos.x = Mathf.Clamp(rectTransform.anchoredPosition.x + cPos.x * speed, -764, 764);
        rectTransform.anchoredPosition = cPos;
        oldPos = newPos;
    }
    
}
