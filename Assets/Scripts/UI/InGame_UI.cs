using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame_UI : MonoBehaviour
{

    public Text[] remain;// remina Dirt, Root, Pebble;
    public Text level;
    public Text remainTryTime;
    
    public Level_UI level_UI;
    public GameObject round;
    public Text roundText;

    public GameObject resumeBtn;

    public Button[] button;
    public GameObject[] block;
    public int currDig = 0; // 0=> swallowly, 1=> deeply
    public Canvas canvas;
    public GameObject[] two;
    Vector3 min, max;
    public Vector3 viewMin, viewMax;
    float yMin, yMax;

    [Header("GameResult UI")]
    public CanvasGroup gameResultCG;
    public Text gameResultText;

    [Header("Lakiaro Progress")]
    public Sprite[] lakiaroSprite;
    public Image[] lakiaroImage;
    public Image[] lakiaroBG;
    public Image progressImage;
    public Text progressText;
    public Image progressGlow;

    public Text timerText;


    void OnDisable()
    {
        Init();
    }

    void Init()
    {
        gameResultCG.gameObject.SetActive(false);
        round.SetActive(false);
        resumeBtn.SetActive(false);
    }

    public RectTransform top, bottom;

    void SetUISize()
    {
        float topH = Camera.main.WorldToScreenPoint(Vector3.up * 12f).y;
        float botH = Camera.main.WorldToScreenPoint(Vector3.zero).y;

        Debug.LogWarning(Screen.height - topH);
        topH = ((Screen.height - topH) / Screen.height) * 1920f;
        botH = (botH / Screen.height) * 1920f;
        two[0].GetComponent<RectTransform>().sizeDelta = new Vector2(two[0].GetComponent<RectTransform>().sizeDelta.x, topH);
        two[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -topH / 2);
        two[1].GetComponent<RectTransform>().sizeDelta = new Vector2(two[1].GetComponent<RectTransform>().sizeDelta.x, botH);
        two[1].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, botH / 2);

        Debug.LogFormat("Screen : ({0},{1}) top SizeDelta : {2} bot SizeDelta : {3}",Screen.width,Screen.height, two[0].GetComponent<RectTransform>().sizeDelta, two[0].GetComponent<RectTransform>().sizeDelta);

    }

    // Start is called before the first frame update
    void Start()
    {
        SetUISize();

        yMin = two[1].GetComponent<RectTransform>().rect.y * 2;
        yMax = two[0].GetComponent<RectTransform>().rect.y * 2;
        min = Vector3.zero;
        min.y = -yMin;
        max.x = two[1].GetComponent<RectTransform>().rect.xMax * 2;
        max.y = 1920f + yMax;

        viewMin = Camera.main.ScreenToViewportPoint(min);
        viewMax = Camera.main.ScreenToViewportPoint(max);
        viewMin = new Vector3(0f, (two[0].GetComponent<RectTransform>().sizeDelta.y / 1920f), 0f);
        viewMax = new Vector3(1f, (1920f - two[1].GetComponent<RectTransform>().sizeDelta.y) / 1920f, 0f);

        Debug.LogWarning(viewMax + " " + viewMin + " " + (two[0].GetComponent<RectTransform>().sizeDelta.y / 1920f) + " " + (1920f - two[1].GetComponent<RectTransform>().sizeDelta.y) / 1920f);

    }
    
    public void UpdateTimer(int time)
    {
        int m, s;
        m = time / 60;
        s = time % 60;
        timerText.text = string.Format("{0:0#}:{1:0#}", m, s);
    }

    public void UpdateRemainTexts(int remainDirt, int remainRoot, int remainPebble, int _remainTryTime, int _currlevel, int _lakiaroLevel, float progress)
    {
        remain[0].text = remainDirt.ToString() + "개";
        remain[1].text = remainRoot.ToString() + "개";
        remain[2].text = remainPebble.ToString() + "개";

        remainTryTime.text = _remainTryTime.ToString();
        level.text = (_currlevel + 1).ToString() + " / " + (_lakiaroLevel + 1);
        level_UI.SetLevel(_currlevel);
        UpdateProgress(progress, _lakiaroLevel);
    }

    Coroutine[] remainCoroutine = new Coroutine[3];
    public void UpdateRemainLakiaroText(int index, int remainNum)
    {
        remain[index].text = remainNum.ToString() + "개";
        if (remainCoroutine[index] != null) StopCoroutine(remainCoroutine[index]);
        else
        {
            Debug.Log(index + " 코루틴 없음");
        }
        remainCoroutine[index] = StartCoroutine(TextGlowEffect(remain[index].GetComponentInChildren<Image>()));
    }

    Coroutine tryTimeCoroutine;
    public void UpdateRemainTryTime(int _remainTryTime)
    {
        remainTryTime.text = _remainTryTime.ToString();
        if (tryTimeCoroutine != null) StopCoroutine(tryTimeCoroutine);
        tryTimeCoroutine = StartCoroutine(TextGlowEffect(remainTryTime.GetComponentInChildren<Image>()));
    }

    public void UpdateLevel(int _currlevel, int _lakiaroLevel)
    {
        level.text = (_currlevel + 1).ToString() + " / " + (_lakiaroLevel + 1);
        level_UI.SetLevel(_currlevel);
    }

    Coroutine progressCoroutine;
    public void UpdateProgress(float progress, int lakiaroLevel)
    {
        float tempProgress = Mathf.Floor(progress * 10) * 0.1f;
        progressText.text = string.Format("{0:0.0}%", tempProgress);
        if(progressCoroutine != null) StopCoroutine(progressCoroutine);
        progressCoroutine = StartCoroutine(TextGlowEffect(progressGlow));
        progressImage.fillAmount = progress * 0.01f;

                int curr = 5;
        if (80 <= progress && progress < 100) curr = 4;
        else if (60 <= progress && progress < 80) curr = 3;
        else if (40 <= progress && progress < 60) curr = 2;
        else if (20 <= progress && progress < 40) curr = 1;
        else if (0 < progress && progress < 20) curr = 0;
        for (int i = 0; i < lakiaroImage.Length; i++)
        {
            if (curr == i)
            {
                lakiaroImage[i].sprite = lakiaroSprite[i + lakiaroLevel];
                lakiaroBG[i].enabled = true;
            }
            else
            {
                lakiaroImage[i].sprite = lakiaroSprite[i + lakiaroLevel + 10];
                lakiaroBG[i].enabled = false;
            }
        }
    }

    public void ChangeDigType()
    {
        button[currDig].enabled = true;
        block[currDig].SetActive(true);

        if (currDig == 0)
        {
            currDig = 1;
        }
        else if (currDig == 1)
        {
            currDig = 0;
        }

        button[currDig].enabled = false;
        block[currDig].SetActive(false);

    }

    public int effect = 0;

    IEnumerator TextGlowEffect(Image glowEffect)
    {
        Debug.Log(glowEffect.name);
        Color color = Color.white;
        glowEffect.color = color;
        float time = 0;
        effect++;
        while (glowEffect.color.a > 0)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp(color.a, 0f, time * 0.5f);
            if (color.a < 0.1f) color.a = 0;
            glowEffect.color = color;
            
            yield return null;
        }
        effect--;
    }

    public void EnableRound()
    {
        if (!round.activeSelf) round.SetActive(true);

        roundText.text = "흙을 모두 찾는데 성공했습니다.\n3";
        StartCoroutine(RoundTimer());
    }

    IEnumerator RoundTimer()
    {
        float timer = 3f;
        while(timer > 0f)
        {
            timer -= Time.deltaTime;
            roundText.text = "흙을 모두 찾는데 성공했습니다.\n" + ((int)timer).ToString(); ;
            yield return null;
        }
    }

    public void DisableRound()
    {
        if (round.activeSelf) round.SetActive(false);

    }

    public void OnResultUI(bool gameResult)
    {
        gameResultCG.alpha = 0;
        gameResultText.text = (gameResult) ? "수확에\n성공하였습니다." : "수확에\n실패하였습니다.";
        gameResultCG.gameObject.SetActive(true);
        StartCoroutine(OnGameResult());
    }
    IEnumerator OnGameResult()
    {
        while (gameResultCG.alpha < 0.95f)
        {
            gameResultCG.alpha = Mathf.Lerp(gameResultCG.alpha, 1, Time.deltaTime);
            yield return null;
        }
        gameResultCG.alpha = 1f;
    }
    public void OffResultUI()
    {
        gameResultCG.gameObject.SetActive(false);
    }

    public void Resume()
    {
        resumeBtn.SetActive(false);
        GameManager.Instance.Resume();
    }
    public void Pause()
    {
        resumeBtn.SetActive(true);
    }

    public GameObject checkUseHintUI;
    public GameObject useHintOverUI;
    public Text hintRemainText;

    public void CheckUseHintUI(int remainTime, int hintCount)
    {
        if(remainTime == 0)
        {
            useHintOverUI.SetActive(true);
        }
        else
        {
            hintRemainText.text = string.Format("{0} / {1}", remainTime, hintCount);
            checkUseHintUI.SetActive(true);
        }
    }

}
