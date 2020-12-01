using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby_UI : MonoBehaviour
{
    public LakiaroBtn[] lakiaroBtn;
    public Sprite[] lakiaroSprite;
    public LaKiaroInfo_UI laKiaroInfo_UI;

    public RectTransform[] panel;
    public RectTransform[] lakiaroPos;


    WaitForSeconds wfsOne = new WaitForSeconds(1f);

    void Start()
    {
        LoadData();
    }

    Color[] colors = { new Color(0, 0, 1, 0.4f), new Color(1, 1, 0, 0.4f), new Color(1, 0, 0, 0.4f) };

    public void LoadData()
    {
        Color setColor;
        Caltime(); // 쿨타임과 리프레시할 라키아로부터 제설정

        for (int i = 0; i < 5; i++)
        {
            if (GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].LakiaroLevel == 4) setColor = colors[2];
            else if (GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].LakiaroLevel > 1) setColor = colors[1];
            else setColor = colors[0];

            lakiaroBtn[i].LoadData(lakiaroSprite[GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].LakiaroLevel], setColor, GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].Pos);
        }

        StartCoroutine(Timer());
    }

    public void Caltime()
    {
        System.TimeSpan timespan;

        for (int i = 0; i < 5; i++)
        {
            if (GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].CurrDigging) continue;
            if (GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].IsDig)
            {
                timespan = System.DateTime.Now - System.Convert.ToDateTime(GameManager.Instance.dataManager.gameData.userIndate);

                GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].CoolTime -= (int)timespan.Seconds;

                if (GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].CoolTime < 0)
                {
                    CreateNewLakiaro(i);
                }
            }
            else
            {
                timespan = System.DateTime.Now - System.Convert.ToDateTime(GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].GenerateTime);
                
                if ((int)timespan.TotalMinutes > 30)
                {
                    Debug.Log("오래되서 재생성" + i);
                    CreateNewLakiaro(i);
                }
            }
        }
    }

    IEnumerator Timer()
    {
        System.TimeSpan timespan;

        while (true)
        {
            yield return wfsOne;

            for(int i = 0; i < 5; i++)
            {
                if (GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].CurrDigging) continue;
                if (GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].IsDig)
                {
                    GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].CoolTime -= 1;

                    if (GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].CoolTime < 0)
                    {
                        CreateNewLakiaro(i);
                    }
                }
                else
                {
                    timespan = System.DateTime.Now - System.Convert.ToDateTime(GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].GenerateTime);

                    if ((int)timespan.TotalMinutes > 30)
                    {
                        Debug.Log("오래되서 재생성" + i);
                        CreateNewLakiaro(i);
                    }
                }
            }

        }
    }


    public void CreateNewLakiaro(int index)
    {
        GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].LakiaroLevel = Random.Range(0, 4);
        GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].Pos = lakiaroPos[5 * index + Random.Range(0, 4)].anchoredPosition;
        GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].CoolTime = 0;
        GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].GenerateTime = System.DateTime.Now.ToString();
        GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].IsDig = false;

        Color setColor;
        if (GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].LakiaroLevel == 4) setColor = colors[2];
        else if (GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].LakiaroLevel > 1) setColor = colors[1];
        else setColor = colors[0];

        lakiaroBtn[index].LoadData(lakiaroSprite[GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].LakiaroLevel], setColor, GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].Pos);
        Debug.Log("??");
    }

    int currindex;
    public void CallLakiaroInfo(int index)
    {
        currindex = index;
        laKiaroInfo_UI.OnInfo(GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].LakiaroLevel, 3, GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].Pos);
        Debug.Log(GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].LakiaroLevel + " " + 3);
    }

    public void OnClickPlayBtn()
    {
        UIManager.Instance.CallInGameUI();
        gameObject.SetActive(false);

        GameManager.Instance.lakiaroManager.StartGame(GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[currindex].LakiaroLevel, 3);

        GameManager.Instance.audioManager.CallAudioClip(1);
    }
}
