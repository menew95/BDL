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

    public void CallPracticeMenu()
    {
        practice_UI_CG.gameObject.SetActive(true);
    }

    public void CallInGameUI()
    {
        main_UI_CG.gameObject.SetActive(false);
        inGame_UI_CG.gameObject.SetActive(true);
    }

    public void CallMainUI()
    {
        main_UI_CG.gameObject.SetActive(true);
        inGame_UI_CG.gameObject.SetActive(false);
    }
}
