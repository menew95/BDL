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
    public LakiaroGameData lakiaroGameData = new LakiaroGameData();
    public LakiaroGameData dailyChallengeData = new LakiaroGameData();

    public PlayerData playerData = new PlayerData();
    public UpgradeData upgradeData = new UpgradeData();
    public StaticData staticData = new StaticData();

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
    private int lakiaroInfoIndex;
    [SerializeField]
    private int lakiaroLevel;
    [SerializeField]
    private int currLevel;
    [SerializeField]
    private int currRemainTryTime, hintCount;
    [SerializeField]
    private int manosHoeDeeplyDig;
    [SerializeField]
    private float progress;
    [SerializeField]
    private float timer = 0;

    public int LakiaroInfoIndex { get => lakiaroInfoIndex; set => lakiaroInfoIndex = value; }
    public List<LakiaroListData> LakiaroRoot { get => lakiaroRoot; set => lakiaroRoot = value; }
    public List<RootList> RootLists { get => rootLists; set => rootLists = value; }
    public int LakiaroLevel { get => lakiaroLevel; set => lakiaroLevel = value; }
    public int CurrLevel { get => currLevel; set => currLevel = value; }
    public int CurrRemainTryTime { get => currRemainTryTime; set => currRemainTryTime = value; }
    public int HintCount { get => hintCount; set => hintCount = value; }
    public int ManosHoeDeeplyDig { get => manosHoeDeeplyDig; set => manosHoeDeeplyDig = value; }
    public float Progress { get => progress; set => progress = value; }
    public float Timer { get => timer; set => timer = value; }
}

[System.Serializable]
public class PlayerData
{
    [SerializeField]
    string userIndate = System.DateTime.Now.ToString();
    [SerializeField]
    bool hasSaveGameData = false;
    [SerializeField]
    long gold = 0;
    [SerializeField]
    bool isDailyChallengeClear = false;
    [SerializeField]
    bool isDailyChallengeCurrDig = false;

    public string UserIndate { get => userIndate; set => userIndate = value; }
    public long Gold { get => gold; set => gold = value; }
    public bool IsDailyChallengeClear { get => isDailyChallengeClear; set => isDailyChallengeClear = value; }
    public bool IsDailyChallengeCurrDig { get => isDailyChallengeCurrDig; set => isDailyChallengeCurrDig = value; }
    public bool HasSaveGameData { get => hasSaveGameData; set => hasSaveGameData = value; }
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
    private List<int> levelData = new List<int> { 0 , 0, 0, 0 };

    public List<int> LevelData { get => levelData; set => levelData = value; }
    public int LakiaroFoundChance { get => LevelData[0]; set => LevelData[0] = value; }
    public int RegenerationCooltime { get => LevelData[1]; set => LevelData[1] = value; }
    public int ManosHoeSwallowlyDig { get => LevelData[2]; set => LevelData[2] = value; }
    public int ManosHoeDeeplyDig { get => LevelData[3]; set => LevelData[3] = value; }
}

[System.Serializable]
public class StaticData
{
    [SerializeField]
    private LakiaroStaticData[] lakiaroStaticData = new LakiaroStaticData[5];

    public LakiaroStaticData[] LakiaroStaticData { get => lakiaroStaticData; set => lakiaroStaticData = value; }
}

[System.Serializable]
public class LakiaroStaticData
{
    [SerializeField]
    private Static_Game static_Game = new Static_Game();
    [SerializeField]
    private Static_Time static_Time = new Static_Time();
    [SerializeField]
    private Static_ETC static_ETC = new Static_ETC();

    public Static_Game Static_Game { get => static_Game; set => static_Game = value; }
    public Static_Time Static_Time { get => static_Time; set => static_Time = value; }
    public Static_ETC Static_ETC { get => static_ETC; set => static_ETC = value; }
}
[System.Serializable]
public class Static_Game
{
    [SerializeField]
    private int found_Lakiaro_Time = 0;
    [SerializeField]
    private int perfect_Dig_Lakiaro_Time = 0;
    [SerializeField]
    private float perfect_Dig_Rate = 0;
    [SerializeField]
    private float min_dameged_Lakiaro_Productivity = 0;
    [SerializeField]
    private float avg_dameged_Lakiaro_Productivity = 0;

    public int Found_Lakiaro_Time { get => found_Lakiaro_Time; set => found_Lakiaro_Time = value; }
    public int Perfect_Dig_Lakiaro_Time { get => perfect_Dig_Lakiaro_Time; set => perfect_Dig_Lakiaro_Time = value; }
    public float Perfect_Dig_Rate { get => perfect_Dig_Rate; set => perfect_Dig_Rate = value; }
    public float Min_dameged_Lakiaro_Productivity { get => min_dameged_Lakiaro_Productivity; set => min_dameged_Lakiaro_Productivity = value; }
    public float Avg_dameged_Lakiaro_Productivity { get => avg_dameged_Lakiaro_Productivity; set => avg_dameged_Lakiaro_Productivity = value; }
}
[System.Serializable]
public class Static_Time
{
    [SerializeField]
    private int min_Dig_Time = 0;
    [SerializeField]
    private int avg_Dig_Time = 0;

    public int Min_Dig_Time { get => min_Dig_Time; set => min_Dig_Time = value; }
    public int Avg_Dig_Time { get => avg_Dig_Time; set => avg_Dig_Time = value; }
}
[System.Serializable]
public class Static_ETC
{
    [SerializeField]
    private int min_Swallowly_Dig_Count = 0;
    [SerializeField]
    private int max_Swallowly_Dig_Count = 0;
    [SerializeField]
    private float avg_Swallowly_Dig_Count = 0;

    public int Min_Swallowly_Dig_Count { get => min_Swallowly_Dig_Count; set => min_Swallowly_Dig_Count = value; }
    public int Max_Swallowly_Dig_Count { get => max_Swallowly_Dig_Count; set => max_Swallowly_Dig_Count = value; }
    public float Avg_Swallowly_Dig_Count { get => avg_Swallowly_Dig_Count; set => avg_Swallowly_Dig_Count = value; }
}