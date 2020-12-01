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

    public SaveGameData saveGameData;

    public PlayerData playerData;

    public List<LakiaroInfoData> LakiaroInfoDataList = new List<LakiaroInfoData> { new LakiaroInfoData(), new LakiaroInfoData(), new LakiaroInfoData(), new LakiaroInfoData(), new LakiaroInfoData() };
}

[System.Serializable]
public class SaveGameData
{
    public Lakiaro[,] lakiaroRoot = new Lakiaro[12, 12];
    [SerializeField]
    private List<RootList> rootLists = new List<RootList>();
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