﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop_UI : MonoBehaviour
{
    private List<int> maxLevelData = new List<int> { 10, 6, 10, 5 };
    private List<List<long>> upgradeGoldData = new List<List<long>>
    {
        new List<long> { 10000000, 25000000, 60000000, 130000000, 260000000, 500000000, 900000000, 1600000000, 2700000000, 4000000000 },
        new List<long> { 10000000, 25000000, 100000000, 500000000, 1500000000, 4000000000 },
        new List<long> { 10000000, 25000000, 60000000, 135000000, 295000000, 610000000, 1180000000, 2185000000, 3780000000, 6124000000 },
        new List<long> { 60000000, 300000000, 1300000000, 4500000000 }
    };
    private List<string> desList = new List<string>
    {
        "고등급 라키아로 발견 확률 증가",
        "라키아로 탐색 시간 감소",
        "얕게파기 횟수 {0}회 증가",
        "깊게 파기시 주변 흙 확인 범위 {0}칸 증가"
    };
    private List<int> desPerList = new List<int>
    {
        5,1,1,1
    };
    private List<bool> desPerList2 = new List<bool>
    {
        true, true, false, false
    };

    public List<Text> levelTextList = new List<Text>();
    public List<GameObject> gold = new List<GameObject>();
    public List<Button> upgradeBtnList = new List<Button>();
    public List<Text> desTextList = new List<Text>();
    
    public GameObject alertObj;
    public Text alertText;

    void OnEnable()
    {
        LoadDataUpgradeData();
    }

    public void LoadDataUpgradeData()
    {
        for(int i = 0; i < levelTextList.Count; i++)
        {
            ChangeUI(i);
        }
    }

    void ChangeUI(int i)
    {
        int level = GameManager.Instance.dataManager.gameData.upgradeData.LevelData[i];
        if (maxLevelData[i] == level)
        {
            levelTextList[i].text = "Lv Max";

            gold[i].SetActive(false);

            upgradeBtnList[i].interactable = false;
            upgradeBtnList[i].GetComponentInChildren<Text>().text = "Max";
            upgradeBtnList[i].GetComponentInChildren<Text>().color = Color.white;
        }
        else
        {
            levelTextList[i].text = string.Format("Lv {0}", level + 1);
            gold[i].GetComponentInChildren<Text>().text = upgradeGoldData[i][level].ToString();
        }

        if (desPerList2[i])
        {
            desTextList[i].text = string.Format(desList[i], level * desPerList[i]);
        }
        else
        {
            desTextList[i].text = string.Format(desList[i], level);
        }
    }

    public void OnClickUpgradeBtn()
    {
        AudioManager.Instance.CallAudioClip(1);
        GameManager.Instance.LoadingStart();
        GameManager.Instance.dataManager.CheckGold(upgradeGoldData[currSelect][GameManager.Instance.dataManager.gameData.upgradeData.LevelData[currSelect]]);
    }

    public void Upgrade()
    {
        GameManager.Instance.ChangeGoldData(-upgradeGoldData[currSelect][GameManager.Instance.dataManager.gameData.upgradeData.LevelData[currSelect]]);
        GameManager.Instance.dataManager.gameData.upgradeData.LevelData[currSelect] += 1;
        ChangeUI(currSelect);
        GameManager.Instance.dataManager.UpdateUpgradeDataOnFirebase();
        GameManager.Instance.dataManager.UpdatePlayerDataDataOnFirebase();
    }

    public void Alert(string msg)
    {
        upgradeAlert.SetActive(false);
        alertObj.SetActive(true);
        alertText.text = msg;
    }

    public GameObject upgradeAlert;
    int currSelect = 0;
    public void OnUpgradeAlert(int index)
    {
        AudioManager.Instance.CallAudioClip(1);
        currSelect = index;
        upgradeAlert.SetActive(true);
    }

    public void OffUpgradeAlert()
    {
        AudioManager.Instance.CallAudioClip(1);
        upgradeAlert.SetActive(false);
    }
}
