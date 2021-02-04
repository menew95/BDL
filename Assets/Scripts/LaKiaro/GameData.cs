using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataInfo;
using System;

[CreateAssetMenu(fileName = "GameDataSO", menuName = "CreateSO/GameDataScriptableObject", order = 2)]
[System.Serializable]
public class GameDataScriptableObject : ScriptableObject
{
    public GameData gameData;
}

[System.Serializable]
public class GameData
{
    public string userIndate;

    public bool isSaveGameData = false;

    public LakiaroGameData lakiaroGameData = new LakiaroGameData();
    public LakiaroGameData dailyChallengeData = new LakiaroGameData();

    public PlayerData playerData = new PlayerData();
    public UpgradeData upgradeData = new UpgradeData();

    public List<LakiaroInfoData> LakiaroInfoDataList = new List<LakiaroInfoData> { new LakiaroInfoData(), new LakiaroInfoData(), new LakiaroInfoData(), new LakiaroInfoData(), new LakiaroInfoData() };

}

[System.Serializable]
public class LakiaroListData
{
    [SerializeField]
    private List<Lakiaro> lakiaroList = new List<Lakiaro>();

    public List<Lakiaro> LakiaroList { get => lakiaroList; set => lakiaroList = value; }
}

[System.Serializable]
public class LakiaroGameData
{
    [SerializeField]
    private List<LakiaroListData> lakiaroRoot = new List<LakiaroListData>();
    [SerializeField]
    private List<RootList> rootLists = new List<RootList>();

    [SerializeField]
    private int lakiaroLevel;
    [SerializeField]
    private int currLevel;
    [SerializeField]
    private int manosHoeLevel;
    [SerializeField]
    private int currRemainTryTime;
    [SerializeField]
    private float progress;
    
    public List<LakiaroListData> LakiaroRoot { get => lakiaroRoot; set => lakiaroRoot = value; }
    public List<RootList> RootLists { get => rootLists; set => rootLists = value; }
    public int LakiaroLevel { get => lakiaroLevel; set => lakiaroLevel = value; }
    public int CurrLevel { get => currLevel; set => currLevel = value; }
    public int ManosHoeLevel { get => manosHoeLevel; set => manosHoeLevel = value; }
    public int CurrRemainTryTime { get => currRemainTryTime; set => currRemainTryTime = value; }
    public float Progress { get => progress; set => progress = value; }
}

[System.Serializable]
public class PlayerData
{
    [SerializeField]
    long gold = 0;
    [SerializeField]
    bool isDailyChallengeClear = false;
    [SerializeField]
    bool isDailyChallengeCurrDig = false;
    
    public long Gold { get => gold; set => gold = value; }
    public bool IsDailyChallengeClear { get => isDailyChallengeClear; set => isDailyChallengeClear = value; }
    public bool IsDailyChallengeCurrDig { get => isDailyChallengeCurrDig; set => isDailyChallengeCurrDig = value; }
}

[System.Serializable]
public class UpgradeData
{

    /* 
     * lakiaroFoundChance 라키아로 고등급 확률 증가
     * regenerationCooltime 라키아로 리젠 타임 감소
     * manosHoeSwallowlyDig 얕게파기 횟수증가
     * manosHoeDeeplyDig 깊게파기 범위 증가
     */

    [SerializeField]
    private List<int> levelData = new List<int> { 1, 1, 1, 1 };

    public List<int> LevelData { get => levelData; set => levelData = value; }
    public int LakiaroFoundChance { get => LevelData[0]; set => LevelData[0] = value; }
    public int RegenerationCooltime { get => LevelData[1]; set => LevelData[1] = value; }
    public int ManosHoeSwallowlyDig { get => LevelData[2]; set => LevelData[2] = value; }
    public int ManosHoeDeeplyDig { get => LevelData[3]; set => LevelData[3] = value; }
}