using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gold_UI : MonoBehaviour
{
    public Text goldText;


    void Start()
    {
        SetCoin();
    }

    public void SetCoin()
    {
        goldText.text = GameManager.Instance.dataManager.gameData.playerData.Gold.ToString();
    }

    public void UpdateCoin()
    {
        StopAllCoroutines();
        StartCoroutine(UpdateCoinText());
    }

    WaitForSeconds wfs = new WaitForSeconds(0.1f); 

    IEnumerator UpdateCoinText()
    {
        long currGold = long.Parse(goldText.text);
        long dis = GameManager.Instance.dataManager.gameData.playerData.Gold - currGold;
        float time = 0;
        
        while(time < 2.0f)
        {
            time += Time.deltaTime;

            //currGold = Mathf.Lerp(currGold, GameManager.Instance.dataManager.gameData.playerData.Gold, Time.deltaTime);

            goldText.text = (currGold + (long)(dis * (time * 0.5f))).ToString();
            yield return null;
        }
        Debug.Log(GameManager.Instance.dataManager.gameData.playerData.Gold);
        goldText.text = (GameManager.Instance.dataManager.gameData.playerData.Gold).ToString();
    }
}
