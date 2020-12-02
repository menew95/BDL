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

    public LakiaroGameData lakiaroGameData;

    public PlayerData playerData;

    public List<LakiaroInfoData> LakiaroInfoDataList = new List<LakiaroInfoData> { new LakiaroInfoData(), new LakiaroInfoData(), new LakiaroInfoData(), new LakiaroInfoData(), new LakiaroInfoData() };
}

[System.Serializable]
public class LakiaroGameData
{
    private Lakiaro[,] lakiaroRoot = new Lakiaro[12, 12];
    [SerializeField]
    private List<RootList> rootLists = new List<RootList>();

    private int lakiaroLevel;
    private int currLevel;
    private int manosHoeLevel;
    private int currRemainTryTime;
    private float progress;

    public Lakiaro[,] LakiaroRoot { get => lakiaroRoot; set => lakiaroRoot = value; }
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
    int gold = 0;
    [SerializeField]
    List<int> manosHoes = new List<int>();

    public int Stack { get => stack; set => stack = value; }
    public int Gold { get => gold; set => gold = value; }
    public List<int> ManosHoes { get => manosHoes; set => manosHoes = value; }
}