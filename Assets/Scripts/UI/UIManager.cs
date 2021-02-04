using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                UIManager[] objs = FindObjectsOfType<UIManager>();
                if (objs.Length > 0)
                {
                    _instance = objs[0];
                }

                if (_instance == null)
                {
                    string goName = typeof(UIManager).ToString();
                    GameObject go = GameObject.Find(goName);
                    if (go == null)
                    {
                        go = new GameObject(goName);

                        _instance = go.AddComponent<UIManager>();
                    }
                }
            }

            return _instance;
        }
    }

    public GameObject main_UI;
    public GameObject inGame_UI;
    public Lobby_UI lobby_UI;
    public NewGame_UI newGame_UI;

    public enum UIState
    {
        Main,
        InGame
    }
    public UIState currUIState = UIState.Main;

    public void CallInGameUI()
    {
        currUIState = UIState.InGame;

        main_UI.SetActive(false);

        inGame_UI.SetActive(true);
    }

    public void CallMainUI()
    {
        currUIState = UIState.Main;
        
        main_UI.SetActive(true);

        inGame_UI.SetActive(false);

    }
}
