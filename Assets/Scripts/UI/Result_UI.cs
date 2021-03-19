using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result_UI : MonoBehaviour
{
    public GameObject skipBtn;
    
    [Header("CanvasGroup")]
    public CanvasGroup result_UI;
    public CanvasGroup resultglow;
    public CanvasGroup successUI;
    public CanvasGroup failUI;
    public CanvasGroup continueBtn;

    [Header("Result Element")]
    public Image lakiaroImage;
    public CanvasGroup lakiaroGlow;
    public Text progressText;
    public Text goldText;
    public GameObject dailyBonusUI;

    public Sprite[] lakiaroSprite;

    void OnDisable()
    {
        Init();
    }

    public void Init()
    {
        continueBtn.alpha = 0;
        continueBtn.blocksRaycasts = false;

        result_UI.alpha = 0;
        successUI.alpha = 0;
        failUI.alpha = 0;

        progressText.text = "100%";
        goldText.text = "";

        skipBtn.SetActive(true);
        dailyBonusUI.SetActive(false);
    }

    public void OnResultUI(int _lakiaroLevel, float _progress, float _gold, bool _gameResult, bool _isDaily, double _dailyBonus)
    {
        lakiaroLevel = _lakiaroLevel;
        lakiaroImage.sprite = lakiaroSprite[lakiaroLevel];
        progress = _progress;
        gold = _gold;
        gameResult = _gameResult;
        isDaily = _isDaily;
        dailyBonus = _dailyBonus;
        goldText.text = "";
        Debug.Log(_gold + " " + gold);
        StartCoroutine(OnResultCG());
    }

    int lakiaroLevel;
    float progress;
    [SerializeField]
    float gold;
    bool gameResult;
    bool isDaily;
    double dailyBonus;

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
        if(gameResult) StartCoroutine(SetSuccessProgress());
        else StartCoroutine(SetFailProgress());
    }

    IEnumerator SetSuccessProgress()
    {
        AudioManager.Instance.CallAudioClip(3);
        float time = 0;
        float currProgressText = 100f;
        float currProgress = 120f;
        int index = lakiaroLevel;
        Debug.Log(index);
        float duration = (100f - progress) / 20f;
        while (time < duration && currProgressText != progress)
        {
            time += Time.deltaTime;
            currProgressText = Mathf.Lerp(100, progress, time / duration);
            currProgressText = Mathf.Floor(currProgressText * 10) * 0.1f;

            if (currProgress - 20f > currProgressText)
            {
                index--;
                currProgress -= 20f;
                StartCoroutine(ProgressGlow());
            }
            /*if (80 <= currProgressText && currProgressText < 100) index = lakiaroLevel - 1;
            else if (60 <= currProgressText && currProgressText < 80) index = lakiaroLevel - 2;
            else if (40 <= currProgressText && currProgressText < 60) index = lakiaroLevel - 3;
            else if (20 <= currProgressText && currProgressText < 40) index = lakiaroLevel - 4;
            else if (0 < currProgressText && currProgressText < 20) index = lakiaroLevel - 5;
            Debug.Log(index);*/
            lakiaroImage.sprite = lakiaroSprite[index];
            progressText.text = string.Format("{0:0.#}%", currProgressText);
            yield return null;
        }

        progressText.text = string.Format("{0:0.#}%", progress);
        StartCoroutine(GameResultCG());
    }

    IEnumerator SetFailProgress()
    {
        AudioManager.Instance.CallAudioClip(4);
        float time = 0;
        float currProgressText = 100f;
        float currProgress = 120f;
        int index = lakiaroLevel;
        Debug.Log(index);
        while (time < 2f && currProgressText != progress)
        {
            time += Time.deltaTime;
            currProgressText = Mathf.Lerp(100, progress, time / 2f);
            currProgressText = Mathf.Floor(currProgressText * 10) * 0.1f;

            if (currProgress - 20f > currProgressText)
            {
                index--;
                currProgress -= 20f;
                StartCoroutine(ProgressGlow());
            }

            lakiaroImage.sprite = lakiaroSprite[index];
            progressText.text = string.Format("{0:0.#}%", currProgressText);
            yield return null;
        }

        lakiaroImage.sprite = lakiaroSprite[index + 10];
        progressText.text = string.Format("{0:0.#}%", progress);
        StartCoroutine(GameResultCG());
    }

    IEnumerator ProgressGlow()
    {
        float time = 0f;
        lakiaroGlow.alpha = 1f;
        while (lakiaroGlow.alpha > 0.05f)
        {
            time += Time.deltaTime;
            lakiaroGlow.alpha = Mathf.Lerp(lakiaroGlow.alpha, 0, time);
            yield return null;
        }
        lakiaroGlow.alpha = 0f;
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
        if(isDaily && gameResult)
        {
            StartCoroutine(SetDailyBonus());
        }
        else
        {
            StartCoroutine(ContinueBtnCG());
        }
    }

    IEnumerator SetDailyBonus()
    {
        dailyBonusUI.SetActive(true);
        float time = 0;
        double bonuseGold = gold * dailyBonus;
        Debug.Log(((int)bonuseGold)+ " " + ((int)gold).ToString() + " " + dailyBonus.ToString());
        while (time < 1.5f)
        {
            time += Time.deltaTime;
            goldText.text = ((int)Mathf.Lerp(gold, (float)bonuseGold, time)).ToString();
            yield return null;
        }


        goldText.text = ((int)bonuseGold).ToString();

        StartCoroutine(ContinueBtnCG());

    }

    IEnumerator ContinueBtnCG()
    {
        while (continueBtn.alpha < 0.95f)
        {
            continueBtn.alpha = Mathf.Lerp(continueBtn.alpha, 1f, Time.deltaTime);
            yield return null;
        }

        continueBtn.blocksRaycasts = true;
    }

    public void Skip()
    {
        StopAllCoroutines();
        skipBtn.SetActive(false);
        result_UI.alpha = 1f;

        lakiaroGlow.alpha = 0f;
        progressText.text = string.Format("{0:0.#}%", progress);
        if (80 <= progress && progress < 100) lakiaroImage.sprite = lakiaroSprite[lakiaroLevel - 1];
        else if (60 <= progress && progress < 80) lakiaroImage.sprite = lakiaroSprite[lakiaroLevel - 2];
        else if (40 <= progress && progress < 60) lakiaroImage.sprite = lakiaroSprite[lakiaroLevel - 3];
        else if (20 <= progress && progress < 40) lakiaroImage.sprite = lakiaroSprite[lakiaroLevel - 4];
        else if (0 < progress && progress < 20) lakiaroImage.sprite = lakiaroSprite[lakiaroLevel - 5];
        else if(progress == 0) lakiaroImage.sprite = lakiaroSprite[lakiaroLevel - 5 + 10];

        ((gameResult) ? successUI : failUI).alpha = 1f;
        AudioManager.Instance.CallAudioClip((gameResult) ? 3 : 4);
        resultglow.alpha = 0f;

        if (isDaily)
        {
            dailyBonusUI.SetActive(true);
            goldText.text = ((int)(gold * dailyBonus)).ToString();
        }
        else
        {
            goldText.text = ((int)gold).ToString();
        }

        continueBtn.alpha = 1f;
        continueBtn.blocksRaycasts = true;
    }
}
