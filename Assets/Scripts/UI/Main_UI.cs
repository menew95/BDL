using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_UI : MonoBehaviour
{
    public void OnClickStartBtn()
    {
        UIManager.Instance.CallPracticeMenu();
    }

    public void OnClickExitBtn()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
