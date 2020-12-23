﻿using System.Collections;
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
    int stack = 0;
    [SerializeField]
    long gold = 0;
    [SerializeField]
    List<int> manosHoes = new List<int>();
    [SerializeField]
    bool isDailyChallengeClear = false;
    [SerializeField]
    bool isDailyChallengeCurrDig = false;

    public int Stack { get => stack; set => stack = value; }
    public long Gold { get => gold; set => gold = value; }
    public List<int> ManosHoes { get => manosHoes; set => manosHoes = value; }
    public bool IsDailyChallengeClear { get => isDailyChallengeClear; set => isDailyChallengeClear = value; }
    public bool IsDailyChallengeCurrDig { get => isDailyChallengeCurrDig; set => isDailyChallengeCurrDig = value; }
}