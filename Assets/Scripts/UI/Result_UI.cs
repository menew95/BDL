using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result_UI : MonoBehaviour
{
    public CanvasGroup result_UI;

    [Header("CanvasGroup")]
    public CanvasGroup resultglow;
    public CanvasGroup successUI;
    public CanvasGroup failUI;

    [Header("Result Element")]
    public Image lakiaroImage;
    public Text progressText;
    public Text goldText;

    public Sprite[] lakiaroSprite;

    void OnDisable()
    {
        Init();
    }

    public void Init()
    {
        result_UI.alpha = 0;
        successUI.alpha = 0;
        failUI.alpha = 0;

        progressText.text = "100%";
        goldText.text = "";
    }

    public void OnResultUI(int _lakiaroLevel, float _progress, float _gold, bool _gameResult)
    {
        lakiaroLevel = _lakiaroLevel;
        lakiaroImage.sprite = lakiaroSprite[lakiaroLevel];
        progress = _progress;
        gold = _gold;
        gameResult = _gameResult;
        goldText.text = "";
        StartCoroutine(OnResultCG());
    }

    int lakiaroLevel;
    float progress;
    [SerializeField]
    float gold;
    bool gameResult;

    IEnumerator OnResultCG()
    {
        float time = 0;
        while (result_UI.alpha < 0.95f)
        {
            time += Time.deltaTime;
            result_UI.alpha = Mathf.Lerp(result_UI.alpha, 1, time * 0.5f);
            yield return null;
        }
        result_UI.alpha = 1f;
        StartCoroutine(SetProgress());
    }

    IEnumerator SetProgress()
    {
        float time = 0;
        float currProgressText = 100f;
        int index = lakiaroLevel;
        Debug.Log(index);
        while (time < 2f && currProgressText != progress)
        {
            time += Time.deltaTime;
            currProgressText = Mathf.Lerp(100, progress, time / 2f);
            currProgressText = Mathf.Floor(currProgressText * 10) * 0.1f;
            if (80 <= currProgressText && currProgressText < 100) index = lakiaroLevel - 1;
            else if (60 <= currProgressText && currProgressText < 80) index = lakiaroLevel - 2;
            else if (40 <= currProgressText && currProgressText < 60) index = lakiaroLevel - 3;
            else if (20 <= currProgressText && currProgressText < 40) index = lakiaroLevel - 4;
            else if (0 < currProgressText && currProgressText < 20) index = lakiaroLevel - 5;
            Debug.Log(index);
            lakiaroImage.sprite = lakiaroSprite[index];
            progressText.text = string.Format("{0:0.#}%", currProgressText);
            yield return null;
        }

        progressText.text = string.Format("{0:0.#}%", progress);
        StartCoroutine(GameResultCG());
    }

    IEnumerator GameResultCG()
    {
        CanvasGroup cg = (gameResult) ? successUI : failUI;
        resultglow.alpha = 1f;
        float time = 0;
        while (cg.alpha < 0.95f)
        {
            time += Time.deltaTime;
            resultglow.alpha = Mathf.Lerp(resultglow.alpha, 0, time);
            cg.alpha = Mathf.Lerp(cg.alpha, 1, time);
            yield return null;
        }
        cg.alpha = 1f;
        StartCoroutine(SetGoldText());
    }
    
    IEnumerator SetGoldText()
    {
        float time = 0;
        while (time  < 1.5f)
        {
            time += Time.deltaTime;
            goldText.text = ((int)Mathf.Lerp(0, gold, time)).ToString();
            yield return null;
        }

        goldText.text = ((int)gold).ToString();
    }

    public void Skip()
    {
        StopAllCoroutines();

        result_UI.alpha = 1f;
        progressText.text = string.Format("{0:0.#}%", progress);
        ((gameResult) ? successUI : failUI).alpha = 1f;
        goldText.text = gold.ToString();
    }
}
