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
    // Start is called before the first frame update
    void Start()
    {
        yMin = two[1].GetComponent<RectTransform>().rect.y * 2;
        yMax = two[0].GetComponent<RectTransform>().rect.y * 2;
        min = Vector3.zero;
        min.y = -yMin;
        max.x = two[1].GetComponent<RectTransform>().rect.xMax * 2;
        max.y = 1920f + yMax;
        viewMin = Camera.main.ScreenToViewportPoint(min);
        viewMax = Camera.main.ScreenToViewportPoint(max);
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Input.mousePosition);
        //Debug.Log(Camera.main.ViewportToScreenPoint(Vector3.one) + " " +yMax + " " + min + " " + max + " " + viewMin + " " + viewMax + " " + Camera.main.ViewportToWorldPoint(viewMin) + " " + Camera.main.ViewportToWorldPoint(viewMax));
    }

    public void UpdateRemainTexts(int remainDirt, int remainRoot, int remainPebble, int _remainTryTime, int _currlevel, int _lakiaroLevel)
    {
        remain[0].text = remainDirt.ToString();
        remain[1].text = remainRoot.ToString();
        remain[2].text = remainPebble.ToString();

        remainTryTime.text = _remainTryTime.ToString();
        level.text = (_currlevel + 1).ToString() + " / " + (_lakiaroLevel + 1);
        level_UI.SetLevel(_currlevel);
    }

    public void UpdateRemainLakiaroText(int index, int remainNum)
    {
        remain[index].text = remainNum.ToString();
        StartCoroutine(TextGlowEffect(remain[index].GetComponentInChildren<Image>()));
    }

    public void UpdateRemainTryTime(int _remainTryTime)
    {
        remainTryTime.text = _remainTryTime.ToString();
        StartCoroutine(TextGlowEffect(remainTryTime.GetComponentInChildren<Image>()));
    }

    public void UpdateLevel(int _currlevel, int _lakiaroLevel)
    {
        level.text = (_currlevel + 1).ToString() + " / " + (_lakiaroLevel + 1);
        level_UI.SetLevel(_currlevel);
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
        Color color = Color.white;
        glowEffect.color = color;

        effect++;
        while (glowEffect.color.a > 0)
        {
            color.a = Mathf.Lerp(color.a, 0f, Time.deltaTime);
            if (color.a < 0.1f) color.a = 0;
            glowEffect.color = color;
            
            yield return null;
        }
        effect--;
    }

    public void EnableRound(bool lastRound)
    {
        if (!round.activeSelf) round.SetActive(true);

        roundText.text = (!lastRound) ? "흙을 모두 찾는데 성공했습니다." : "라키아로를 수확하였습니다.";
    }

    public void DisableRound()
    {
        if (round.activeSelf) round.SetActive(false);

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
}
