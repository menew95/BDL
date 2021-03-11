using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameManager[] objs = FindObjectsOfType<GameManager>();
                if (objs.Length > 0)
                {
                    _instance = objs[0];
                }

                if (_instance == null)
                {
                    string goName = typeof(GameManager).ToString();
                    GameObject go = GameObject.Find(goName);
                    if (go == null)
                    {
                        go = new GameObject(goName);

                        _instance = go.AddComponent<GameManager>();
                    }
                }
            }

            return _instance;
        }
    }

    public AudioManager audioManager;

    public LakiaroManager lakiaroManager;

    public GoogleManager googleManager;
    public DataManager dataManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (UIManager.Instance.currUIState == UIManager.UIState.Main)
            {
                if (UIManager.Instance.lobby_UI.currState == Lobby_UI.LobbyState.Home)
                {
                    dataManager.SaveGameData();
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                }
                else if (UIManager.Instance.lobby_UI.currState == Lobby_UI.LobbyState.NewGame)
                {
                    if (UIManager.Instance.newGame_UI.currState == NewGame_UI.State.None)
                    {
                        UIManager.Instance.lobby_UI.CallHomeUI();
                    }
                    else if (UIManager.Instance.newGame_UI.currState == NewGame_UI.State.InfoUI)
                    {
                        UIManager.Instance.newGame_UI.GetComponent<NewGame_UI>().OffLakiaroInfo();
                    }
                    else if (UIManager.Instance.newGame_UI.currState == NewGame_UI.State.DailyInfo)
                    {
                        UIManager.Instance.newGame_UI.GetComponent<NewGame_UI>().OffDailyInfo();
                    }
                    else if (UIManager.Instance.newGame_UI.currState == NewGame_UI.State.loadingUI)
                    {
                        UIManager.Instance.newGame_UI.GetComponent<NewGame_UI>().OffLoading();
                    }
                    else if (UIManager.Instance.newGame_UI.currState == NewGame_UI.State.CurrDinggingUI)
                    {
                        UIManager.Instance.newGame_UI.GetComponent<NewGame_UI>().OffCurrDigging();
                    }
                }
                else if (UIManager.Instance.lobby_UI.currState == Lobby_UI.LobbyState.Shop)
                {
                    UIManager.Instance.lobby_UI.CallHomeUI();
                }
                else if (UIManager.Instance.lobby_UI.currState == Lobby_UI.LobbyState.Static)
                {
                    UIManager.Instance.lobby_UI.CallHomeUI();
                }


                /*if (Input.GetKeyUp(KeyCode.Escape))
                {
                    dataManager.SaveGameData();
    #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
    #else
                    Application.Quit();
    #endif
                }*/
            }
            else if (UIManager.Instance.currUIState == UIManager.UIState.InGame)
            {
                if (lakiaroManager.gamePause)
                {
                    SaveGame();
                }
                else
                {
                    Pause();
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            UIManager.Instance.newGame_UI.GetComponent<NewGame_UI>().result_UI.Skip();
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            dataManager.CheckGold(2000);
        }
    }

    public void Resume()
    {
        lakiaroManager.gamePause = false;
    }

    public void Pause()
    {
        lakiaroManager.gamePause = true;
        lakiaroManager.clickPause = true;
        lakiaroManager.inGame_UI.Pause();
    }
    public void AdPause()
    {
        lakiaroManager.gamePause = true;
        lakiaroManager.clickPause = true;
    }
    void SaveGame()
    {
        UIManager.Instance.CallMainUI();
        lakiaroManager.SaveGameData();
    }

    public int tempLakiaroLevel = 4;
    public int tempManos = 4;
    public float tempProgress = 100;
    public bool tempGameResult = true;
    public bool tempDaily = false;

    public void FinishDigLakiaro(int _lakiaroLevel, float _progress, bool _gameResult, bool _isDaily, int _timer, int _currRemainTryTime)
    {
        /*라키아로 가치에 따른 자원 추가
         */
        int lakiaroLevel = _lakiaroLevel + 5;
        int gold = 0;
        if (80 <= _progress && _progress < 100) lakiaroLevel -= 1;
        else if (60 <= _progress && _progress < 80)lakiaroLevel -= 2;
        else if (40 <= _progress && _progress < 60)lakiaroLevel -= 3;
        else if (20 <= _progress && _progress < 40)lakiaroLevel -= 4;
        else if (0 < _progress && _progress < 20) lakiaroLevel -= 5;

        switch (lakiaroLevel)
        {
            case 0:
                gold = 100000;
                break;
            case 1:
                gold = 300000;
                break;
            case 2:
                gold = 700000;
                break;
            case 3:
                gold = 1000000;
                break;
            case 4:
                gold = 1500000;
                break;
            case 5:
                gold = 3000000;
                break;
            case 6:
                gold = 5000000;
                break;
            case 7:
                gold = 10000000;
                break;
            case 8:
                gold = 30000000;
                break;
            case 9:
                gold = 100000000;
                break;
            default:
                gold = 0;
                break;
        }

        if (_isDaily)
        {
            int difficult = _lakiaroLevel - (int)((dataManager.gameData.upgradeData.ManosHoeSwallowlyDig +
                (dataManager.gameData.upgradeData.ManosHoeDeeplyDig * 2)) * 0.5f);
            double dailyBonus = 1.1d;
            switch (difficult)
            {
                case 2:
                    dailyBonus = 2d;
                    break;
                case 1:
                    dailyBonus = 1.6d;
                    break;
                case 0:
                    dailyBonus = 1.3d;
                    break;
                case -1:
                    dailyBonus = 1.1d;
                    break;
            }
            dataManager.gameData.playerData.Gold += (long)(gold * dailyBonus);
            dataManager.gameData.playerData.IsDailyChallengeClear = true;
            dataManager.gameData.playerData.IsDailyChallengeCurrDig = false;
        }
        else
        {
            dataManager.gameData.playerData.Gold += gold;
            dataManager.gameData.playerData.HasSaveGameData = false;

            dataManager.SaveLakiaroInfoData();
        }
        dataManager.UpdatePlayerDataDataOnFirebase();
        dataManager.AddStaticData(_lakiaroLevel, _progress, _timer, _currRemainTryTime);
    }

    public void ChangeGoldData(long gold)
    {
        dataManager.gameData.playerData.Gold += gold;
        UIManager.Instance.lobby_UI.gold_UI.UpdateCoin();
    }
}
