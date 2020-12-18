using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby_UI : MonoBehaviour
{
    public RectTransform map;
    public LakiaroBtn[] lakiaroBtn;
    public Sprite[] lakiaroSprite;
    public LaKiaroInfo_UI laKiaroInfo_UI;
    public Gold_UI gold_UI;
    public Result_UI result_UI;
    public GameObject currDigging;
    public RectTransform currDigAnim;

    public RectTransform[] panel;
    public RectTransform[] lakiaroPos;
    public RectTransform loading;

    public enum State
    {
        None,
        loadingUI,
        InfoUI,
        CurrDinggingUI,
    }
    public State currState;

    WaitForSeconds wfsOne = new WaitForSeconds(1f);

    void OnEnable()
    {
        StopAllCoroutines();
        LoadData();
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
        currDigging.SetActive(false);
    }

    Color[] colors = { new Color(0, 0, 1, 0.4f), new Color(1, 1, 0, 0.4f), new Color(1, 0, 0, 0.4f) };

    public void LoadData()
    {
        Color setColor;
        Caltime(); // 쿨타임과 리프레시할 라키아로부터 제설정

        for (int i = 0; i < 5; i++)
        {
            if (GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].CurrDigging) OnDigAnim(i);
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
        System.TimeSpan timespan;

        for (int i = 0; i < 5; i++)
        {
            if (GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].CurrDigging) continue;
            if (GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].IsDig)
            {
                timespan = System.DateTime.Now - System.Convert.ToDateTime(GameManager.Instance.dataManager.gameData.userIndate);
                GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].CoolTime -= (int)timespan.TotalSeconds;
                
                if (GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].CoolTime < 0)
                {
                    Debug.Log("쿨타임 끝 라키아로 생성");
                    CreateNewLakiaro(i);
                }
                else
                {

                    Debug.Log("cooltime");
                    lakiaroBtn[i].OnLoading();
                }
            }
            else
            {
                timespan = System.DateTime.Now - System.Convert.ToDateTime(GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].GenerateTime);
                
                if ((int)timespan.TotalMinutes > 30)
                {
                    Debug.Log("오래되서 재생성" + i);
                    CreateNewLakiaro(i);
                }
            }
        }
        
        GameManager.Instance.dataManager.gameData.userIndate = System.DateTime.Now.ToString();
    }

    IEnumerator Timer() // cooltime 및 오래된 라키아로 재생성 타이머
    {
        System.TimeSpan timespan;
        while (true)
        {
            yield return wfsOne;
            for (int i = 0; i < 5; i++)
            {
                if (GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].CurrDigging) continue;
                if (GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].IsDig)
                {
                    GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].CoolTime -= 1;

                    if (GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].CoolTime < 0)
                    {
                        CreateNewLakiaro(i);
                    }
                }
                else
                {
                    timespan = System.DateTime.Now - System.Convert.ToDateTime(GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].GenerateTime);

                    if ((int)timespan.TotalMinutes > 30)
                    {
                        Debug.Log("오래되서 재생성" + i);
                        CreateNewLakiaro(i);
                    }
                }
            }

        }
    }


    public void CreateNewLakiaro(int index) // 새로운 라키아로 infodata 설정 및 btn에 적용
    {
        GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].LakiaroLevel = Random.Range(0, 4);
        GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].Pos = lakiaroPos[5 * index + Random.Range(0, 4)].anchoredPosition;
        GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].CoolTime = 0;
        GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].GenerateTime = System.DateTime.Now.ToString();
        GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].IsDig = false;

        Color setColor;
        if (GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].LakiaroLevel == 4) setColor = colors[2];
        else if (GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].LakiaroLevel > 1) setColor = colors[1];
        else setColor = colors[0];

        lakiaroBtn[index].SetData(lakiaroSprite[GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].LakiaroLevel], setColor, GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].Pos);
    }

    public void OnClickPlayBtn()
    {
        bool currDigging = false;
        int index = 0;
        for(int i = 0; i < 5; i++)
        {
            if(GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].CurrDigging)
            {
                if (currindex == i) break;
                index = i;
                currDigging = true;
                break;
            }
        }
        if (!currDigging)
        {
            StartNewGame();
        }
        else
        {
            OnCurrDigging();
            laKiaroInfo_UI.gameObject.SetActive(false);
        }
    }

    public void DeleteGameData() // 기존 게임 데이타를 지우고 새로운 게임 시작
    {
        for(int i = 0; i < 5; i++)
        {
            if (GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].CurrDigging)
            {
                Debug.LogWarning("ssssssssssssSSS");
                GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].CurrDigging = false;
                GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].IsDig = true;
                GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[i].CoolTime = 30;
                break;
            }
        }

        StartNewGame();
    }

    public void StartNewGame() // 새로운 게임 시작
    {
        UIManager.Instance.CallInGameUI();
        gameObject.SetActive(false);
        GameManager.Instance.lakiaroManager.StartGame(GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[currindex].LakiaroLevel, 3, GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[currindex].CurrDigging);

        GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[currindex].CurrDigging = true;

        GameManager.Instance.audioManager.CallAudioClip(1);
    }

    public void DigFinishLakiaro() 
    {
        /* 메인게임이 끝난후 적용할 효과
         * infodata (isDig, cooltime) 설정
         * 라키아로 버튼 로딩버튼으로 변경
         * 게임 결과창 On
         */
        lakiaroBtn[currindex].OnLoading();

        GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[currindex].IsDig = true;
        GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[currindex].CoolTime = 600;

    }

    public void OnResultUI(int lakiaroLevel, float _progress, float _gold, bool _gameResult)
    {
        result_UI.gameObject.SetActive(true);
        result_UI.OnResultUI(lakiaroLevel, _progress, _gold, _gameResult);
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
        currState = State.InfoUI;
        currindex = index;
        laKiaroInfo_UI.OnInfo(GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].LakiaroLevel, 3, GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].Pos, GameManager.Instance.dataManager.gameData.LakiaroInfoDataList[index].CurrDigging);
        loading.gameObject.SetActive(false);
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameManager.Instance.dataManager.gameData.playerData.Gold += 100;
            gold_UI.UpdateCoin();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameManager.Instance.dataManager.gameData.playerData.Gold += 1000;
            gold_UI.UpdateCoin();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GameManager.Instance.dataManager.gameData.playerData.Gold += 10000;
            gold_UI.UpdateCoin();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            GameManager.Instance.dataManager.gameData.playerData.Gold += 100000;
            gold_UI.UpdateCoin();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            GameManager.Instance.dataManager.gameData.playerData.Gold += 1000000;
            gold_UI.UpdateCoin();
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            GameManager.Instance.dataManager.gameData.playerData.Gold -= 100;
            gold_UI.UpdateCoin();
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            GameManager.Instance.dataManager.gameData.playerData.Gold -= 1000;
            gold_UI.UpdateCoin();
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            GameManager.Instance.dataManager.gameData.playerData.Gold -= 1000;
            gold_UI.UpdateCoin();
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            GameManager.Instance.dataManager.gameData.playerData.Gold -= 100000;
            gold_UI.UpdateCoin();
        }
    }
}
