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

    public DataManager dataManager;

    void Update()
    {
        if (UIManager.Instance.currUIState == UIManager.UIState.Main)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                dataManager.SaveGameData();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }
        else if (UIManager.Instance.currUIState == UIManager.UIState.Lobby)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                if (UIManager.Instance.lobby_UI.GetComponent<Lobby_UI>().currState == Lobby_UI.State.None)
                {
                    UIManager.Instance.CallMainUI();
                }
                else if (UIManager.Instance.lobby_UI.GetComponent<Lobby_UI>().currState == Lobby_UI.State.InfoUI)
                {
                    UIManager.Instance.lobby_UI.GetComponent<Lobby_UI>().OffLakiaroInfo();
                }
                else if (UIManager.Instance.lobby_UI.GetComponent<Lobby_UI>().currState == Lobby_UI.State.loadingUI)
                {
                    UIManager.Instance.lobby_UI.GetComponent<Lobby_UI>().OffLoading();
                }
                else if (UIManager.Instance.lobby_UI.GetComponent<Lobby_UI>().currState == Lobby_UI.State.CurrDinggingUI)
                {
                    UIManager.Instance.lobby_UI.GetComponent<Lobby_UI>().OffCurrDigging();
                }
            }
        }

        
        else if (UIManager.Instance.currUIState == UIManager.UIState.Practice)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                UIManager.Instance.ExitPracticeMenu();
            }

        }
        else if (UIManager.Instance.currUIState == UIManager.UIState.InGame)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
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
        if (Input.GetKeyUp(KeyCode.R))
        {
            FinishDigLakiaro(tempLakiaroLevel, tempProgress, tempGameResult);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            UIManager.Instance.lobby_UI.GetComponent<Lobby_UI>().result_UI.Skip();
        }
    }

    public void Resume()
    {
        lakiaroManager.gamePause = false;
    }

    public void Pause()
    {
        lakiaroManager.gamePause = true;
        lakiaroManager.inGame_UI.Pause();
    }

    void SaveGame()
    {
        UIManager.Instance.CallLobbyUI();
        lakiaroManager.SaveGameData();
    }

    public int tempLakiaroLevel = 4;
    public float tempProgress = 100;
    public bool tempGameResult = true;

    public void FinishDigLakiaro(int _lakiaroLevel, float _progress, bool _gameResult)
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

        dataManager.gameData.playerData.Gold += gold;
        UIManager.Instance.lobby_UI.GetComponent<Lobby_UI>().OnResultUI(_lakiaroLevel + 5, _progress, (_gameResult) ? gold : 0, _gameResult);
    }
}
