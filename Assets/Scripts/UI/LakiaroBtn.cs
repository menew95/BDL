using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LakiaroBtn : MonoBehaviour
{
    public Image lakiaroImage, bgcolor;
    public GameObject loadingBar;
    public Text coolTimeText;
    Color blackColor = new Color(0, 0, 0, 150f / 255f);

    public void OnLoading()
    {
        if (!loadingBar.activeSelf) loadingBar.SetActive(true);
        if (!coolTimeText.gameObject.activeSelf) coolTimeText.gameObject.SetActive(true);
        if (lakiaroImage.gameObject.activeSelf) lakiaroImage.gameObject.SetActive(false);

        bgcolor.color = blackColor;
    }

    public void SetData(Sprite lakiaroSprite, Color color, Vector3 anchoredPos)
    {

        if(loadingBar.activeSelf) loadingBar.SetActive(false);
        if (coolTimeText.gameObject.activeSelf) coolTimeText.gameObject.SetActive(false);
        if (!lakiaroImage.gameObject.activeSelf) lakiaroImage.gameObject.SetActive(true);

        lakiaroImage.sprite = lakiaroSprite;

        bgcolor.color = color;

        GetComponent<RectTransform>().anchoredPosition = anchoredPos;
    }

    public void ChangeCoolTimeText(int coolTime)
    {
        int minutes, seconds;
        minutes = coolTime / 60;
        seconds = coolTime % 60;
        coolTimeText.text = string.Format("{0}:{1:0#}", minutes, seconds);
    }
}
