using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public GameData gameData;

    void Awake()
    {
        LoadGameDataObject();
    }

    public void LoadGameDataObject()
    {
        string dataPath;
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        dataPath = Application.streamingAssetsPath + "/SaveData.json";
#elif UNITY_ANDROID
        dataPath = Application.persistentDataPath + "/SaveData.json";
#elif UNITY_IOS
        dataPath = "file://" + Application.streamingAssetsPath + "/SaveData.json";
#endif
        if (File.Exists(dataPath))
        {
            Debug.Log("로드시작");
            string fromJsonData = File.ReadAllText(dataPath);
            gameData = JsonUtility.FromJson<GameData>(fromJsonData);
            Debug.Log("로드 성공");
        }
        else
        {
            Debug.Log("새로운 데이터 생성");
            gameData = new GameData();
        }
    }

    public void SaveGameData()
    {
        gameData.userIndate = System.DateTime.Now.ToString();

        string toJsonData = JsonUtility.ToJson(gameData);
        string dataPath;
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        dataPath = Application.streamingAssetsPath + "/SaveData.json";
#elif UNITY_ANDROID
        dataPath = Application.persistentDataPath + "/SaveData.json";
#elif UNITY_IOS
        dataPath = "file://" + Application.streamingAssetsPath + "/SaveData.json";
#endif
        File.WriteAllText(dataPath, toJsonData);
        Debug.Log("데이터 저장 완료");
    }

    private void OnAppliicationQuit()
    {
        SaveGameData();
    }

    public void NewDailyData()
    {
        gameData.dailyChallengeData.LakiaroLevel = Random.Range(2, 4);
        gameData.dailyChallengeData.ManosHoeLevel = Mathf.Clamp(Random.Range(gameData.dailyChallengeData.LakiaroLevel - 2, gameData.dailyChallengeData.LakiaroLevel + 1), 0, 4);

        gameData.playerData.IsDailyChallengeClear = false;
        gameData.playerData.IsDailyChallengeCurrDig = false;
    }
}
