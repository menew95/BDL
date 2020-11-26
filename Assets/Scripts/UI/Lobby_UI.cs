using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Lobby_UI : MonoBehaviour
{
    public List<DataInfo.LakiaroInfoData> lakiaroInfoDatas;
    public RectTransform[] lakiaroBtn;
    public Sprite[] lakiaroSprite;
    public LaKiaroInfo_UI laKiaroInfo_UI;


    void Start()
    {
        LoadData();
    }

    Color[] colors = { new Color(0, 0, 1, 0.4f), new Color(1, 1, 0, 0.4f), new Color(1, 0, 0, 0.4f) };

    public void LoadData()
    {
        lakiaroInfoDatas = GameManager.Instance.dataManager.gameData.LakiaroInfoDataList;
        for (int i = 0; i < 5; i++)
        {
            lakiaroBtn[i].GetChild(0).GetComponent<Image>().sprite = lakiaroSprite[lakiaroInfoDatas[i].LakiaroLevel];
            if (lakiaroInfoDatas[i].LakiaroLevel == 4) lakiaroBtn[i].GetComponent<Image>().color = colors[2];
            else if (lakiaroInfoDatas[i].LakiaroLevel > 1) lakiaroBtn[i].GetComponent<Image>().color = colors[1];
            else lakiaroBtn[i].GetComponent<Image>().color = colors[0];

            lakiaroInfoDatas[i].Pos = lakiaroBtn[i].anchoredPosition;
        }
    }

    public void CalCooltime()
    {

    }

    int currindex;
    public void CallLakiaroInfo(int index)
    {
        currindex = index;
        laKiaroInfo_UI.OnInfo(lakiaroInfoDatas[index].LakiaroLevel, 3, lakiaroInfoDatas[index].Pos);
        Debug.Log(lakiaroInfoDatas[index].LakiaroLevel + " " + 3);
    }

    public void OnClickPlayBtn()
    {
        UIManager.Instance.CallInGameUI();
        gameObject.SetActive(false);

        GameManager.Instance.lakiaroManager.StartGame(lakiaroInfoDatas[currindex].LakiaroLevel, 3);

        GameManager.Instance.audioManager.CallAudioClip(1);
    }
}
