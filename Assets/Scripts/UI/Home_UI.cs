using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Home_UI : MonoBehaviour
{

    public GameObject resumeGameBtn;
    public GameObject dailyPlayBtn;
    public Text dailyTimeText;

    // Start is called before the first frame update
    void OnEnable()
    {
        if (GameManager.Instance.dataManager.gameData.hasSaveGameData)
        {
            OnResumeBtn();
        }
        else
        {
            resumeGameBtn.SetActive(false);
        }

        dailyTimeText.text = System.DateTime.Now.ToString("MM월 dd일");

        if (GameManager.Instance.dataManager.gameData.playerData.IsDailyChallengeClear)
        {
            dailyPlayBtn.SetActive(false);
        }
        else
        {
            dailyPlayBtn.SetActive(true);
            if (GameManager.Instance.dataManager.gameData.playerData.IsDailyChallengeCurrDig)
            {
                dailyPlayBtn.GetComponentInChildren<Text>().text = "계속하기";
            }
            else
            {
                dailyPlayBtn.GetComponentInChildren<Text>().text = "플레이";
            }
        }
    }

    void OnResumeBtn()
    {
        resumeGameBtn.SetActive(true);
        Text resumeText = resumeGameBtn.transform.GetChild(1).GetComponent<Text>();
        string lakiaro = "";
        switch (GameManager.Instance.dataManager.gameData.lakiaroGameData.LakiaroLevel)
        {
            case 0:
                lakiaro = "고급";
                break;
            case 1:
                lakiaro = "굵은";
                break;
            case 2:
                lakiaro = "희귀";
                break;
            case 3:
                lakiaro = "숙성";
                break;
            case 4:
                lakiaro = "원시";
                break;
        }
        int s, m;
        s = (int)GameManager.Instance.dataManager.gameData.lakiaroGameData.Timer % 60;
        m = (int)GameManager.Instance.dataManager.gameData.lakiaroGameData.Timer / 60;

        resumeText.text = string.Format("{0:0#}:{1:0#} - {2} 라키아로", m, s, lakiaro);

    }

    public void StartDailyLakiaro()
    {
        UIManager.Instance.CallInGameUI();
        gameObject.SetActive(false);

        GameManager.Instance.lakiaroManager.GameSetting(5, GameManager.Instance.dataManager.gameData.dailyChallengeData.ManosHoeLevel, GameManager.Instance.dataManager.gameData.playerData.IsDailyChallengeCurrDig, true);

        GameManager.Instance.dataManager.gameData.playerData.IsDailyChallengeCurrDig = true;

        GameManager.Instance.audioManager.CallAudioClip(1);
    }

    public void OnClickResumeGameBtn()
    {
        UIManager.Instance.CallInGameUI();

        GameManager.Instance.lakiaroManager.LoadGameData();

        GameManager.Instance.audioManager.CallAudioClip(1);
    }
}
