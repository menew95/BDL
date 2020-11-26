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
        UIManager.Instance.CallMainUI();
    }
}
