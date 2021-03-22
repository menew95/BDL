using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGame_UI : MonoBehaviour
{
    public RectTransform map;
    public LakiaroBtn[] lakiaroBtn;
    public Sprite[] lakiaroSprite;
    public LaKiaroInfo_UI laKiaroInfo_UI;
    public LaKiaroInfo_UI dailyInfo_UI;
    public Gold_UI gold_UI;
    public Result_UI result_UI;
    public GameObject currDigging;
    public RectTransform currDigAnim;

    public GameObject dailyClear;
    public GameObject dailyLoading;

    public RectTransform[] panel;
    public RectTransform[] lakiaroPos;
    public RectTransform loading;

    public GameObject helper2Obj;

    public enum State
    {
        None,
        loadingUI,
        InfoUI,
        DailyInfo,
        CurrDinggingUI,
        Helper
    }
    public State currState;

    WaitForSeconds wfsOne = new WaitForSeconds(1f);

    void OnEnable()
    {
        StopAllCoroutines();
        LoadData();
        if (!GameManager.Instance.dataManager.gameData.playerData.HelperEvent2)
        {
            GameManager.Instance.dataManager.gameData.playerData.HelperEvent2 = true;
            CallHelper2();
        }
    }

    void OnDisable()
    {
        InitLobbyUI();
    }

    void InitLobbyUI()
    {
        currState = State.None;
        map.anchoredPosition = Vector3.zero;
        currDigAnim.gameObject.SetActive(false);
        loading.gameObject.SetActive(false);
        laKiaroInfo_UI.gameObject.SetActive(false);
        dailyInfo_UI.gameObject.SetActive(false);
        currDigging.SetActive(false);
    }

    Color[] colors = { new Color(0, 0, 1, 0.4f), new Color(1, 1, 0, 0.4f), new Color(1, 0, 0, 0.4f) };

    public void LoadData()
    {
        Debug.Log("??");
        Color setColor;
        Caltime(); // 쿨타임과 리프레시할 라키아로부터 제설정

        if (GameManager.Instance.dataManager.gameData.playerData.HasSaveGameData) OnDigAnim(GameManager.Instance.dataManager.gameData.lakiaroGameData.LakiaroInfoIndex);

        for (int i = 0; i < 5; i++)
        {
            if (GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].IsDig) continue;
            if (GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].LakiaroLevel == 4) setColor = colors[2];
            else if (GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].LakiaroLevel > 1) setColor = colors[1];
            else setColor = colors[0];

            lakiaroBtn[i].SetData(lakiaroSprite[GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].LakiaroLevel], setColor, GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].Pos);
        }

        StartCoroutine(Timer());
    }

    public void Caltime() // 이전 접속한 시간과 새로 접속한 시간을 비교후 coolttime과 오래된 라키아로 재생성
    {
        Debug.Log("Caltime");
        System.TimeSpan timespan;

        for (int i = 0; i < 5; i++)
        {
            if (GameManager.Instance.dataManager.gameData.playerData.HasSaveGameData)
            {
                if (GameManager.Instance.dataManager.gameData.lakiaroGameData.LakiaroInfoIndex == i) continue;
            }
            if (GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].IsDig)
            {
                Debug.Log(i + " : 라키아로 파져있음");
                timespan = System.DateTime.Now - System.Convert.ToDateTime(GameManager.Instance.dataManager.gameData.playerData.UserIndate);
                GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].CoolTime -= (int)timespan.TotalSeconds;

                if (GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].CoolTime < 0)
                {
                    Debug.Log(i + " : 라키아로 쿨타임 완료 재생성");
                    CreateNewLakiaro(i);
                }
                else
                {
                    Debug.Log(i + " : 라키아로 재생성중");
                    lakiaroBtn[i].OnLoading();
                }
            }
            else
            {
                timespan = System.DateTime.Now - System.Convert.ToDateTime(GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].GenerateTime);

                if ((int)timespan.TotalMinutes > 120)
                {
                    CreateNewLakiaro(i);
                }
            }
        }

        // 데일리 데이타 재설정
        timespan = System.DateTime.Now - System.Convert.ToDateTime(GameManager.Instance.dataManager.gameData.playerData.UserIndate);
        if (timespan.Days > 0)
        {
            GameManager.Instance.dataManager.NewDailyData();
        }

        GameManager.Instance.dataManager.gameData.playerData.UserIndate = System.DateTime.Now.ToString();
    }

    IEnumerator Timer() // cooltime 및 오래된 라키아로 재생성 타이머
    {
        System.TimeSpan timespan;
        while (true)
        {
            yield return wfsOne;
            for (int i = 0; i < 5; i++)
            {
                if (GameManager.Instance.dataManager.gameData.playerData.HasSaveGameData)
                {
                    if (GameManager.Instance.dataManager.gameData.lakiaroGameData.LakiaroInfoIndex == i) continue;
                }
                if (GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].IsDig)
                {
                    GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].CoolTime -= 1;

                    if (GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].CoolTime < 0)
                    {
                        CreateNewLakiaro(i);
                    }
                    else
                    {
                        lakiaroBtn[i].ChangeCoolTimeText(GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].CoolTime);
                    }
                }
                else
                {
                    timespan = System.DateTime.Now - System.Convert.ToDateTime(GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].GenerateTime);

                    if ((int)timespan.TotalMinutes > 120)
                    {
                        CreateNewLakiaro(i);
                    }
                }
            }

        }
    }

    int[][] percent = new int[][]
    {
        new int[]{0, 0, 5, 20, 100 },
        new int[]{1, 6, 16, 36, 100 },
        new int[]{2, 8, 21, 46, 100 },
        new int[]{3, 10, 26, 56, 100 },
        new int[]{4, 12, 31, 66, 100 },
        new int[]{5, 14, 36, 76, 100 },
        new int[]{6, 17, 42, 80, 100 },
        new int[]{7, 20, 48, 80, 100 },
        new int[]{8, 23, 54, 80, 100 },
        new int[]{9, 26, 54, 80, 100 },
        new int[]{10, 30, 55, 80, 100 }
    };

    public void CreateNewLakiaro(int index) // 새로운 라키아로 infodata 설정 및 btn에 적용
    {
        var ran = Random.Range(0, 100);
        var lakiaroLevel = 0;
        for (int i = 0; i < 5; i++)
        {
            if (ran < percent[GameManager.Instance.dataManager.gameData.upgradeData.LakiaroFoundChance][i])
            {
                lakiaroLevel = 4 - i;
                break;
            }
        }
        GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].LakiaroLevel = lakiaroLevel;
        GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].Pos = lakiaroPos[5 * index + Random.Range(0, 4)].anchoredPosition;
        GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].CoolTime = 0;
        GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].GenerateTime = System.DateTime.Now.ToString();
        GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].IsDig = false;

        Color setColor;
        if (GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].LakiaroLevel == 4) setColor = colors[2];
        else if (GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].LakiaroLevel > 1) setColor = colors[1];
        else setColor = colors[0];

        lakiaroBtn[index].SetData(lakiaroSprite[GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].LakiaroLevel], setColor, GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].Pos);

        GameManager.Instance.dataManager.UpdateLakiaroInfoDataOnFirebase(index);
    }

    public void OnClickPlayBtn()
    {
        if (GameManager.Instance.dataManager.gameData.playerData.HasSaveGameData)
        {
            if (GameManager.Instance.dataManager.gameData.lakiaroGameData.LakiaroInfoIndex == currindex)
            {
                StartGame(true);
            }
            else
            {
                OnCurrDigging();
                laKiaroInfo_UI.gameObject.SetActive(false);
            }
        }
        else
        {
            StartGame(false);
        }
    }

    public void DeleteGameData() // 기존 게임 데이타를 지우고 새로운 게임 시작
    {
        int index = GameManager.Instance.dataManager.gameData.lakiaroGameData.LakiaroInfoIndex;
        GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].IsDig = true;
        GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].CoolTime = 1800 - (GameManager.Instance.dataManager.gameData.upgradeData.RegenerationCooltime * 120);

        StartGame(false);
    }

    public void StartGame(bool isLoad) // 라키아로 불러오거나 새로운 게임 로딩 밎 광고 로딩
    {
        UIManager.Instance.CallInGameUI();
        GameManager.Instance.lakiaroManager.GameSetting(GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[currindex].LakiaroLevel, isLoad, false);
        GameManager.Instance.dataManager.gameData.lakiaroGameData.LakiaroInfoIndex = currindex;
        if (0.25f > Random.Range(0f, 1f))
        {
            GameManager.Instance.googleAdsManager.ShowInterstitialAd();
        }
        else
        {
            GameManager.Instance.lakiaroManager.StartGame();
        }

        AudioManager.Instance.CallAudioClip(1);
    }

    public void DigFinishLakiaro(int lakiaroLevel, float _progress, float _gold, bool _gameResult, bool _isDaily, double dailyBonus)
    {
        /* 메인게임이 끝난후 적용할 효과
         * infodata (isDig, cooltime) 설정
         * 라키아로 버튼 로딩버튼으로 변경
         * 게임 결과창 On
         */

        if (!_isDaily)
        {
            lakiaroBtn[currindex].OnLoading();
            
        }

        result_UI.gameObject.SetActive(true);
        result_UI.OnResultUI(lakiaroLevel, _progress, _gold, _gameResult, _isDaily, dailyBonus);
    }

    public void OnResultUI(int lakiaroLevel, float _progress, float _gold, bool _gameResult, bool isDaily = false, double dailyBonus = 0)
    {
        result_UI.gameObject.SetActive(true);
        result_UI.OnResultUI(lakiaroLevel, _progress, _gold, _gameResult, isDaily, dailyBonus);
    }

    public void OffResultUI()
    {
        result_UI.gameObject.SetActive(false);
        gold_UI.UpdateCoin();
    }
    /* UI On Off 시 필요한 작업들
    * currState변경 및 UI 위치 변경 등
    */

    int currindex; // 현재 선택된 Index => info data index값
    public void CallLakiaroInfo(int index)
    {
        OffDailyInfo();
        currState = State.InfoUI;
        currindex = index;
        if (GameManager.Instance.dataManager.gameData.playerData.HasSaveGameData)
        {
            laKiaroInfo_UI.OnInfo(GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].LakiaroLevel, 3, GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].Pos, GameManager.Instance.dataManager.gameData.lakiaroGameData.LakiaroInfoIndex == index);
        }
        else
        {
            laKiaroInfo_UI.OnInfo(GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].LakiaroLevel, 3, GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].Pos, false);
        }

        loading.gameObject.SetActive(false);
    }

    public void CallLakiaroDailyInfo()
    {
        if (GameManager.Instance.dataManager.gameData.playerData.IsDailyChallengeClear)
        {
            dailyClear.SetActive(true);
            return;
        }

        int lakiaro = GameManager.Instance.dataManager.gameData.dailyChallengeData.LakiaroLevel;
        bool IsDailyChallengeCurrDig = GameManager.Instance.dataManager.gameData.playerData.IsDailyChallengeCurrDig;

        dailyInfo_UI.OnInfo(lakiaro, GameManager.Instance.dataManager.gameData.upgradeData.ManosHoeSwallowlyDig, Vector3.zero, GameManager.Instance.dataManager.gameData.playerData.IsDailyChallengeCurrDig);

        OffLakiaroInfo();
        loading.gameObject.SetActive(false);
        currState = State.DailyInfo;
    }

    public void OffDailyInfo()
    {
        currState = State.None;
        dailyInfo_UI.OffInfo();
    }

    public void OffLakiaroInfo()
    {
        currState = State.None;
        laKiaroInfo_UI.OffInfo();
    }

    public void OnLoading(int index)
    {
        currState = State.loadingUI;
        laKiaroInfo_UI.gameObject.SetActive(false);

        loading.gameObject.SetActive(true);
        loading.anchoredPosition = GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].Pos;
    }

    public void OffLoading()
    {
        currState = State.None;
        loading.gameObject.SetActive(false);
    }

    void OnCurrDigging()
    {
        currState = State.CurrDinggingUI;
        currDigging.SetActive(true);
    }

    public void OffCurrDigging()
    {
        currState = State.InfoUI;
        currDigging.SetActive(false);
        laKiaroInfo_UI.gameObject.SetActive(true);
    }

    void OnDigAnim(int index)
    {
        currDigAnim.gameObject.SetActive(true);
        currDigAnim.SetParent(lakiaroBtn[index].transform);
        currDigAnim.anchoredPosition = Vector3.zero;
    }

    public void CallHelper2()
    {
        currState = State.Helper;
        helper2Obj.SetActive(true);
    }

    public void OffHelper2()
    {
        currState = State.None;
        helper2Obj.SetActive(false);
    }
}
