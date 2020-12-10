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
        float currGold = int.Parse(goldText.text);
        float time = 0;
        float temp = 1;
        while(Mathf.Abs(GameManager.Instance.dataManager.gameData.playerData.Gold - currGold) > 1)
        {
            Debug.Log(currGold);
            time += Time.deltaTime;
            currGold = Mathf.Lerp(currGold, GameManager.Instance.dataManager.gameData.playerData.Gold, 0.08f);

            goldText.text = ((int)currGold).ToString();
            yield return null;
        }
        currGold = GameManager.Instance.dataManager.gameData.playerData.Gold;
        goldText.text = ((int)currGold).ToString();
    }
}
