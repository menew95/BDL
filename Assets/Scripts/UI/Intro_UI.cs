using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro_UI : MonoBehaviour
{
    public RectTransform BG;

    void OnEnable()
    {

    }

    void OnDisable()
    {

    }

    public void OnClickStartBtn()
    {
        gameObject.SetActive(false);

        UIManager.Instance.lobby_UI.CallHomeUI();
        UIManager.Instance.lobby_UI.gameObject.SetActive(true);

        AudioManager.Instance.CallAudioClip(1);
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
