using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaKiaroInfo_UI : MonoBehaviour
{
    public Image lakiaroImage;
    public Text lakiaroNmaeText;
    public Text difficultText;

    public Sprite[] lakiaroImages;
    public string[] lakiaroNmae = { "고급 라키아로", "굵은 라키아로", "희귀 라키아로", "숙성 라키아로", "원시 라키아로" };
    public string[] difficults = { "매우 어려움", "어려움", "보통", "쉬움", "매우 쉬움" };

    Vector3 infoUIPos;

    public void OnInfo(int lakiaroLevel, int hoeLevel, Vector3 pos)
    {
        gameObject.SetActive(true);
        SetInfo(lakiaroLevel, hoeLevel);
        SetPos(pos);
    }

    public void OffInfo()
    {
        gameObject.SetActive(true);
    }

    Color[] colors = { new Color(50f/255f, 150f/255f, 1), new Color(1, 220f/250f, 0), new Color(1, 100f/255f, 0) };
    void SetInfo(int lakiaroLevel, int hoeLevel)
    {
        int difficult = 0;
        lakiaroImage.sprite = lakiaroImages[lakiaroLevel];
        lakiaroNmaeText.text = lakiaroNmae[lakiaroLevel];
        if (lakiaroLevel == 4) lakiaroNmaeText.color = colors[2];
        else if (lakiaroLevel > 1) lakiaroNmaeText.color = colors[1];
        else lakiaroNmaeText.color = colors[0];
        

        if (lakiaroLevel < hoeLevel - 1) difficult = 4;
        else if (lakiaroLevel < hoeLevel) difficult = 3;
        else if (lakiaroLevel == hoeLevel) difficult = 2;
        else if (lakiaroLevel > hoeLevel) difficult = 1;
        else if (lakiaroLevel > hoeLevel + 1) difficult = 0;
        difficultText.text = difficults[difficult];
    }

    public void SetPos(Vector3 pos)
    {
        GetComponent<RectTransform>().anchoredPosition = pos;

        infoUIPos = transform.position;
        
    }
}
