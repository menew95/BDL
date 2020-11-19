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

    public CanvasGroup main_UI_CG;
    public CanvasGroup practice_UI_CG;
    public CanvasGroup inGame_UI_CG;

    public enum UIState
    {
        Main,
        Practice,
        InGame
    }
    public UIState currUIState = UIState.Main;

    public void CallPracticeMenu()
    {
        currUIState = UIState.Practice;

        practice_UI_CG.gameObject.SetActive(true);
    }

    public void ExitPracticeMenu()
    {
        currUIState = UIState.Main;

        practice_UI_CG.GetComponent<Practice_UI>().OnExitPlayBtn();
    }

    public void CallInGameUI()
    {
        currUIState = UIState.InGame;

        main_UI_CG.gameObject.SetActive(false);
        inGame_UI_CG.gameObject.SetActive(true);
    }

    public void CallMainUI()
    {
        currUIState = UIState.Main;

        main_UI_CG.gameObject.SetActive(true);
        inGame_UI_CG.gameObject.SetActive(false);
    }
}
