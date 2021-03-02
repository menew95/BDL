using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop_UI : MonoBehaviour
{
    private List<int> maxLevelData = new List<int> { 6, 6, 10, 5 };
    private List<List<long>> upgradeGoldData = new List<List<long>>
    {
        new List<long> { 5000000,25000000,100000000,500000000, 2500000000 },
        new List<long> { 5000000,25000000,100000000,500000000, 2500000000 },
        new List<long> { 10000000, 25000000,60000000,140000000, 300000000, 640000000, 1270000000, 2420000000, 4360000000 },
        new List<long> { 60000000, 300000000, 1300000000, 4500000000 }
    };
    private List<string> desList = new List<string>
    {
        "고등급 라키아로 발견 확률 {0}% 증가",
        "라키아로 재생성 시간 {0}% 감소",
        "얕게파기 횟수 {0}회 증가",
        "깊게 파기시 주변 흙 확인 범위 {0}칸 증가"
    };
    private List<int> desPerList = new List<int>
    {
        5,5,1,1
    };
    private List<bool> desPerList2 = new List<bool>
    {
        true, true, false, false
    };

    public List<Text> levelTextList = new List<Text>();
    public List<GameObject> gold = new List<GameObject>();
    public List<Button> upgradeBtnList = new List<Button>();
    public List<Text> desTextList = new List<Text>();

    void OnEnable()
    {
        LoadDataUpgradeData();
    }

    void Awake()
    {
        
    }

    public void LoadDataUpgradeData()
    {
        for(int i = 0; i < levelTextList.Count; i++)
        {
            if(maxLevelData[i] == GameManager.Instance.dataManager.gameData.upgradeData.LevelData[i])
            {
                levelTextList[i].text = "Lv Max";

                gold[i].SetActive(false);

                upgradeBtnList[i].interactable = false;
                upgradeBtnList[i].GetComponentInChildren<Text>().text = "Max";
                upgradeBtnList[i].GetComponentInChildren<Text>().color = Color.white;
            }
            else
            {
                Debug.Log(GameManager.Instance.dataManager.gameData.upgradeData.LevelData[i]);
                levelTextList[i].text = string.Format("Lv {0}", GameManager.Instance.dataManager.gameData.upgradeData.LevelData[i]);
                gold[i].GetComponentInChildren<Text>().text = upgradeGoldData[i][GameManager.Instance.dataManager.gameData.upgradeData.LevelData[i] - 1].ToString();
            }

            if (desPerList2[i])
            {
                desTextList[i].text = string.Format(desList[i], GameManager.Instance.dataManager.gameData.upgradeData.LevelData[i] * desPerList[i]);
            }
            else
            {
                desTextList[i].text = string.Format(desList[i], GameManager.Instance.dataManager.gameData.upgradeData.LevelData[i]);
            }

        }
    }

    public void OnClickUpgradeBtn()
    {
        if (GameManager.Instance.dataManager.gameData.playerData.Gold >=
            upgradeGoldData[currSelect][GameManager.Instance.dataManager.gameData.upgradeData.LevelData[currSelect] - 1])
        {
            GameManager.Instance.ChnageGoldData(-upgradeGoldData[currSelect][GameManager.Instance.dataManager.gameData.upgradeData.LevelData[currSelect] - 1]);
            GameManager.Instance.dataManager.gameData.upgradeData.LevelData[currSelect] += 1;
            ChangeUI(currSelect);
            GameManager.Instance.dataManager.UpdateUpgradeDataOnFirebase();
        }
    }

    void ChangeUI(int i)
    {
        if (maxLevelData[i] == GameManager.Instance.dataManager.gameData.upgradeData.LevelData[i])
        {
            levelTextList[i].text = "Lv Max";

            gold[i].SetActive(false);

            upgradeBtnList[i].interactable = false;
            upgradeBtnList[i].GetComponentInChildren<Text>().text = "Max";
            upgradeBtnList[i].GetComponentInChildren<Text>().color = Color.white;
        }
        else
        { 
            Debug.Log(GameManager.Instance.dataManager.gameData.upgradeData.LevelData[i]);
            levelTextList[i].text = string.Format("Lv {0}", GameManager.Instance.dataManager.gameData.upgradeData.LevelData[i]);
            gold[i].GetComponentInChildren<Text>().text = upgradeGoldData[i][GameManager.Instance.dataManager.gameData.upgradeData.LevelData[i] - 1].ToString();
        }
    }

    public GameObject upgradeAlert;
    int currSelect = 0;
    public void OnUpgradeAlert(int index)
    {
        currSelect = index;
        upgradeAlert.SetActive(true);
    }

    public void OffUpgradeAlert()
    {
        upgradeAlert.SetActive(false);
    }
}
