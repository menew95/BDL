using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Helper_UI : MonoBehaviour
{
    public RectTransform[] first;
    public RectTransform[] second;

    public GameObject[] helper;
    public InGame_UI inGame_UI;

    public void SetHelperSize(float topH, float botH)
    {
        Debug.Log(topH + " " + botH);
        first[0].GetComponent<RectTransform>().sizeDelta = new Vector2(first[0].GetComponent<RectTransform>().sizeDelta.x, topH);
        first[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -topH / 2);
        first[1].GetComponent<RectTransform>().sizeDelta = new Vector2(first[0].GetComponent<RectTransform>().sizeDelta.x, 1920-topH);
        first[1].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (1920 - topH) / 2);

        second[0].GetComponent<RectTransform>().sizeDelta = new Vector2(second[1].GetComponent<RectTransform>().sizeDelta.x, 1920-botH);
        second[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -(1920f - botH) / 2);
        second[1].GetComponent<RectTransform>().sizeDelta = new Vector2(second[1].GetComponent<RectTransform>().sizeDelta.x, botH);
        second[1].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, botH / 2);
    }

    int curr = 0;
    public void Next()
    {
        if (curr == 2)
        {
            curr = 0;
            inGame_UI.Resume();
            inGame_UI.OffHelper();
            helper[0]?.SetActive(true);
            helper[2]?.SetActive(false);
            return;
        }
        helper[curr++]?.SetActive(false);
        helper[curr]?.SetActive(true);
        
    }
}
