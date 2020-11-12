using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_UI : MonoBehaviour
{
    public RectTransform BG;

    void OnEnable()
    {

    }

    void OnDisable()
    {

    }

    IEnumerator ScrollingBG()
    {
        // 360 랜덤

        float ranDir = Random.Range(0, 360);

        while (true)
        {
            yield return null;
        }
    }

    public void OnClickStartBtn()
    {
        UIManager.Instance.CallPracticeMenu();

        GameManager.Instance.audioManager.CallAudioClip(1);
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
