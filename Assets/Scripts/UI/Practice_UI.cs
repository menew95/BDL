using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Practice_UI : MonoBehaviour
{
    private int currManos = -1; // 현재 선택된 마노스 호미 난이도
    private int currLakiaro = -1; // 현재 선택된 라키아로 난이도
    
    public GameObject[] ManosHoeBtns;
    public GameObject[] LakiaroBtns;
    public GameObject startBtn;

    private CanvasGroup cg;

    public Text[] currDifficultyText;

    void Awake()
    {
        cg = GetComponent<CanvasGroup>();
    }

    void Start()
    {

    }

    void Update()
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnClickCancleBtn();
            }
            else if (Input.GetKeyDown(KeyCode.Home))
            {

            }
            else if (Input.GetKeyDown(KeyCode.Menu))
            {

            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnClickCancleBtn();
            }
            else if (Input.GetKeyDown(KeyCode.Home))
            {

            }
            else if (Input.GetKeyDown(KeyCode.Menu))
            {

            }
        }
    }

    public void Init()
    {
        currLakiaro = -1;
        currManos = -1;
        currDifficultyText[0].text = "";
        currDifficultyText[1].text = "";

        startBtn.SetActive(false);

        for (int i = 0; i < ManosHoeBtns.Length; i++)
        {
            ManosHoeBtns[i].GetComponent<Image>().enabled = false;
        }
        for (int i = 0; i < ManosHoeBtns.Length; i++)
        {
            LakiaroBtns[i].GetComponent<Image>().enabled = false;
        }

        gameObject.SetActive(false);
    }

    public void OnClickCancleBtn()
    {
        Init();
        AudioManager.Instance.CallAudioClip(1);
    }

    public void OnClickManosHoe(int index = 0)
    {
        ChagneManosHoe(index);

        if (!startBtn.activeSelf) startBtn.SetActive(true);

        currManos = index;

        switch (index)
        {
            case 0:
                currDifficultyText[0].text = 18.ToString();
                break;
            case 1:
                currDifficultyText[0].text = 20.ToString();
                break;
            case 2:
                currDifficultyText[0].text = 22.ToString();
                break;
            case 3:
                currDifficultyText[0].text = 25.ToString();
                break;
            case 4:
                currDifficultyText[0].text = 28.ToString();
                break;
        }
        AudioManager.Instance.CallAudioClip(1);
    }

    public void OnClickLakiaroBtn(int index = 0)
    {
        ChangeLakiaro(index);

        if (!startBtn.activeSelf) startBtn.SetActive(true);

        currLakiaro = index;

        currDifficultyText[1].text = LakiaroBtns[index].name;

        AudioManager.Instance.CallAudioClip(1);
    }

    public void ChagneManosHoe(int index = 0) // 호미 버튼을 눌렀을때 적용되는 효과
    {
        for(int i = 0; i < ManosHoeBtns.Length; i++)
        {
            ManosHoeBtns[i].GetComponent<Image>().enabled = (i == index) ? true : false;
        }
    }

    public void ChangeLakiaro(int index = 0) // 라키아로 버튼을 눌렀을때 적용되는 효과
    {
        for (int i = 0; i < ManosHoeBtns.Length; i++)
        {
            LakiaroBtns[i].GetComponent<Image>().enabled = (i == index) ? true : false;
        }
    }

    public CanvasGroup playBtnCG;
    public GameObject playText;

    public LakiaroManager lm;

    public void OnClickPlayBtn()
    {
        if(currLakiaro == -1 && currManos == -1) Debug.Log("Setting" + currLakiaro + currManos);
        else if (currLakiaro == -1) Debug.Log("Lakiaro" + currLakiaro + currManos);
        else if (currManos == -1) Debug.Log("Manos" + currLakiaro + currManos);
        else
        {
            //UIManager.Instance.lobby_UI.GetComponent<Lobby_UI>().CallLakiaroInfo();
            gameObject.SetActive(false);
            Init();
        }

        AudioManager.Instance.CallAudioClip(1);
    }

    public void OnEnterPlayBtn()
    {
        playBtnCG.alpha = 0.2f;
        playText.SetActive(true);
    }

    public void OnExitPlayBtn()
    {
        playBtnCG.alpha = 1f;
        playText.SetActive(false);
    }
}
