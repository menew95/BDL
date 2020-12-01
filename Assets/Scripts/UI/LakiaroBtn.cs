using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LakiaroBtn : MonoBehaviour
{
    public Image lakiaroImage, bgcolor;

    public void LoadData(Sprite lakiaroSprite, Color color, Vector3 anchoredPos)
    {
        lakiaroImage.sprite = lakiaroSprite;

        bgcolor.color = color;

        GetComponent<RectTransform>().anchoredPosition = anchoredPos;
    }
}
