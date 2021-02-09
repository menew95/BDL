using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Static_UI : MonoBehaviour
{
    public RectTransform main;
    public int currIndex = 0;

    [Header("Game Statics")]
    public Text[] found_Lakiaro_Times;
    public Text[] perfect_Dig_Lakiaro_Times;
    public Text[] perfect_Dig_Rate;
    public Text[] min_dameged_Lakiaro_Productivity;
    public Text[] avg_dameged_Lakiaro_Productivity;

    [Header("Time Statics")]
    public Text[] min_Dig_Times;
    public Text[] avg_Dig_Times;

    [Header("ETC Statics")]
    public Text[] min_Swallowly_Dig_Counts;
    public Text[] max_Swallowly_Dig_Counts;
    public Text[] avg_Swallowly_Dig_Counts;

    void OnEnable()
    {
        UIManager.Instance.GetComponent<CanvasScaler>().matchWidthOrHeight = 0f;
        SetStaticData();
    }

    void OnDisable()
    {
        UIManager.Instance.GetComponent<CanvasScaler>().matchWidthOrHeight = 1f;
    }

    float initPos;

    void Awake()
    {
        initPos = main.anchoredPosition.x;
    }
    public void OnClickTapBtn(int index)
    {
        StopAllCoroutines();
        /*Vector3 pos = main.anchoredPosition;
        pos.x += 1080 * (currIndex - index);
        main.anchoredPosition = pos;*/
        if (currIndex == index) return;
        currIndex = index;
        StartCoroutine(Snap(index));
    }

    IEnumerator Snap(int index)
    {
        Vector3 pos = main.anchoredPosition;

        float time = 0f;
        while(time < 0.2f)
        {
            pos.x = Mathf.Lerp( pos.x, initPos - (index * 1080), 0.5f);
            main.anchoredPosition = pos;
            yield return null;
        }
        pos.x = initPos - (index * 1080);
        main.anchoredPosition = pos;
    }

    void SetStaticData()
    {
        for(int i = 0; i < 5; i++)
        {
            SetDataOnGameStatic(i);
            SetDataOnTimeStatic(i);
            SetDataOnETCStatic(i);
        }
    }

    void SetDataOnGameStatic(int index)
    {
        found_Lakiaro_Times[index].text = string.Format("{0}회",GameManager.Instance.dataManager.gameData.staticData.LakiaroStaticData[index].Static_Game.Found_Lakiaro_Time);
        perfect_Dig_Lakiaro_Times[index].text = string.Format("{0}회", GameManager.Instance.dataManager.gameData.staticData.LakiaroStaticData[index].Static_Game.Perfect_Dig_Lakiaro_Time);
        perfect_Dig_Rate[index].text = string.Format("{0}회", GameManager.Instance.dataManager.gameData.staticData.LakiaroStaticData[index].Static_Game.Perfect_Dig_Rate);
        min_dameged_Lakiaro_Productivity[index].text = string.Format("{0}%", GameManager.Instance.dataManager.gameData.staticData.LakiaroStaticData[index].Static_Game.Min_dameged_Lakiaro_Productivity);
        avg_dameged_Lakiaro_Productivity[index].text= string.Format("{0}%",GameManager.Instance.dataManager.gameData.staticData.LakiaroStaticData[index].Static_Game.Avg_dameged_Lakiaro_Productivity);
    }

    void SetDataOnTimeStatic(int index)
    {
        int s, m;
        m = GameManager.Instance.dataManager.gameData.staticData.LakiaroStaticData[index].Static_Time.Min_Dig_Time / 60;
        s = GameManager.Instance.dataManager.gameData.staticData.LakiaroStaticData[index].Static_Time.Min_Dig_Time % 60;
        min_Dig_Times[index].text = string.Format("{0:0#}:{1:0#}", m, s);

        m = GameManager.Instance.dataManager.gameData.staticData.LakiaroStaticData[index].Static_Time.Avg_Dig_Time / 60;
        s = GameManager.Instance.dataManager.gameData.staticData.LakiaroStaticData[index].Static_Time.Avg_Dig_Time % 60;
        avg_Dig_Times[index].text = string.Format("{0:0#}:{1:0#}", m, s);

    }

    void SetDataOnETCStatic(int index)
    {
        min_Swallowly_Dig_Counts[index].text = string.Format("{0}회", GameManager.Instance.dataManager.gameData.staticData.LakiaroStaticData[index].Static_ETC.Min_Swallowly_Dig_Count);
        max_Swallowly_Dig_Counts[index].text = string.Format("{0}회", GameManager.Instance.dataManager.gameData.staticData.LakiaroStaticData[index].Static_ETC.Max_Swallowly_Dig_Count);
        avg_Swallowly_Dig_Counts[index].text = string.Format("{0}회", GameManager.Instance.dataManager.gameData.staticData.LakiaroStaticData[index].Static_ETC.Avg_Swallowly_Dig_Count);

    }
}
