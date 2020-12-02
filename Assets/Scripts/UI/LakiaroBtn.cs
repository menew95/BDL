using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LakiaroBtn : MonoBehaviour
{
    public Image lakiaroImage, bgcolor;
    public GameObject loadingBar;

    Color blackColor = new Color(0, 0, 0, 150f / 255f);

    public void OnLoading()
    {
        if (!loadingBar.activeSelf) loadingBar.SetActive(true);
        if (lakiaroImage.gameObject.activeSelf) lakiaroImage.gameObject.SetActive(false);

        bgcolor.color = blackColor;
    }

    public void SetData(Sprite lakiaroSprite, Color color, Vector3 anchoredPos)
    {

        if(loadingBar.activeSelf) loadingBar.SetActive(false);
        if (!lakiaroImage.gameObject.activeSelf) lakiaroImage.gameObject.SetActive(true);

        lakiaroImage.sprite = lakiaroSprite;

        bgcolor.color = color;

        GetComponent<RectTransform>().anchoredPosition = anchoredPos;
    }
}
