using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;

using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Firebase.Extensions;

public class DataManager : MonoBehaviour
{
    public GameData gameData;

    public delegate void DelGetDB(object data);
    DatabaseReference reference;

    void Awake()
    {
        //.();
    }

    public void ConnectDatabase()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://bdlakiaro-default-rtdb.firebaseio.com");

        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void SetData(string _userId, string _dataName, int _value)
    {
        reference.Child("users").Child(_userId).Child(_dataName).SetValueAsync(_value);
    }

    public void ApplyData(string _userId)
    {
        reference.Child("users").Child(_userId).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                
                Debug.LogWarning(snapshot.GetRawJsonValue());

                gameData = JsonUtility.FromJson<GameData>(snapshot.GetRawJsonValue());
            }
        });
    }
    public UnityEngine.UI.Text text;
    public void GetData(string _userId, string _dataName, DelGetDB _funcDB)
    {
        reference.Child("users").Child(_userId).Child(_dataName).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                _funcDB(snapshot.Value);
            }
            else if (task.IsFaulted)
            {
                text.text = "데이터 로딩 실패";
                Debug.LogWarning("데이터 로딩 실패");
            }
            else
            {
                text.text = "데이터 로딩 취소";
                Debug.LogWarning("데이터 로딩 취소");
            }
        });
    }

    void HandleChildAdded(object sender, ChildChangedEventArgs args)
    {
        if(args.DatabaseError != null)
        {
            return;
        }
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
        Debug.Log(dataPath);
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
        gameData.dailyChallengeData.LakiaroLevel = UnityEngine.Random.Range(2, 4);
        gameData.dailyChallengeData.ManosHoeLevel = Mathf.Clamp(UnityEngine.Random.Range(gameData.dailyChallengeData.LakiaroLevel - 2, gameData.dailyChallengeData.LakiaroLevel + 1), 0, 4);

        gameData.playerData.IsDailyChallengeClear = false;
        gameData.playerData.IsDailyChallengeCurrDig = false;
    }

    public void AddStaticData(int lakiaroLevel, float progress, int time, int remainSwallowCount)
    {
        // Static_Game 데이터 추가

        gameData.staticData.LakiaroStaticData[lakiaroLevel].Static_Game.Found_Lakiaro_Time += 1;
        if(!(progress < 100f))
        {
            gameData.staticData.LakiaroStaticData[lakiaroLevel].Static_Game.Perfect_Dig_Lakiaro_Time += 1;
        }
        gameData.staticData.LakiaroStaticData[lakiaroLevel].Static_Game.Perfect_Dig_Rate =
            (gameData.staticData.LakiaroStaticData[lakiaroLevel].Static_Game.Perfect_Dig_Lakiaro_Time / gameData.staticData.LakiaroStaticData[lakiaroLevel].Static_Game.Found_Lakiaro_Time) * 100f;
        if(gameData.staticData.LakiaroStaticData[lakiaroLevel].Static_Game.Min_dameged_Lakiaro_Productivity > progress)
        {
            gameData.staticData.LakiaroStaticData[lakiaroLevel].Static_Game.Min_dameged_Lakiaro_Productivity = progress;
        }
        gameData.staticData.LakiaroStaticData[lakiaroLevel].Static_Game.Avg_dameged_Lakiaro_Productivity =
            ((gameData.staticData.LakiaroStaticData[lakiaroLevel].Static_Game.Avg_dameged_Lakiaro_Productivity * (gameData.staticData.LakiaroStaticData[lakiaroLevel].Static_Game.Found_Lakiaro_Time - 1)) + progress)
            / (float)gameData.staticData.LakiaroStaticData[lakiaroLevel].Static_Game.Found_Lakiaro_Time * 100f;

        // Static_Time 데이터 추가

        if(gameData.staticData.LakiaroStaticData[lakiaroLevel].Static_Time.Min_Dig_Time > time)
        {
            gameData.staticData.LakiaroStaticData[lakiaroLevel].Static_Time.Min_Dig_Time = time;
        }
        gameData.staticData.LakiaroStaticData[lakiaroLevel].Static_Time.Avg_Dig_Time =
            (int)((((gameData.staticData.LakiaroStaticData[lakiaroLevel].Static_Time.Avg_Dig_Time * (float)(gameData.staticData.LakiaroStaticData[lakiaroLevel].Static_Game.Found_Lakiaro_Time - 1)) + time)
            / gameData.staticData.LakiaroStaticData[lakiaroLevel].Static_Game.Found_Lakiaro_Time) * 100f);

        // Static_ETC 데이터 추가

        if(gameData.staticData.LakiaroStaticData[lakiaroLevel].Static_ETC.Min_Swallowly_Dig_Count > remainSwallowCount)
        {
            gameData.staticData.LakiaroStaticData[lakiaroLevel].Static_ETC.Min_Swallowly_Dig_Count = remainSwallowCount;
        }
        if (gameData.staticData.LakiaroStaticData[lakiaroLevel].Static_ETC.Max_Swallowly_Dig_Count < remainSwallowCount)
        {
            gameData.staticData.LakiaroStaticData[lakiaroLevel].Static_ETC.Max_Swallowly_Dig_Count = remainSwallowCount;
        }

    }

    // 임시
    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    bool isFirebaseInitialized = false;
    public void StartFirebase()
    {
        ConnectDatabase();
        Debug.LogError("test StartFirebase");
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
                GetPlayerData(GameManager.Instance.googleManager.GetFirebaseUserID());
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependenciesL " + dependencyStatus);
            }
        });
    }

    void InitializeFirebase()
    {
        FirebaseApp app = FirebaseApp.DefaultInstance;
        StartListener();
        isFirebaseInitialized = true;
    }

    void StartListener()
    {
        FirebaseDatabase.DefaultInstance.GetReference("GameData").Child(GameManager.Instance.googleManager.GetFirebaseUserID()).ValueChanged += (object sender2, ValueChangedEventArgs e2) =>
        {
            if (e2.DatabaseError != null)
            {
                Debug.LogError(e2.DatabaseError.Message);
                return;
            }
            Debug.Log("Receuved valus for Users.");
            if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0)
            {
                GetPlayerData(GameManager.Instance.googleManager.GetFirebaseUserID());
                Debug.Log("Data is Change");
            }
            else
            {
                Debug.Log("data is not Exist");
            }
        };
    }

    void OnApplicationQuit()
    {
        FirebaseDatabase.DefaultInstance.GetReference("Users").ValueChanged -= null;
    }

    public void AddNewPlayer(string _UserID, string _DisplayName)
    {
        DatabaseReference gameDataRef = FirebaseDatabase.DefaultInstance.GetReference("GameData");
        DatabaseReference usersRef = FirebaseDatabase.DefaultInstance.GetReference("Users");

        string gameDataKey = gameDataRef.Push().Key;
        gameDataRef.Child(gameDataKey).SetRawJsonValueAsync(JsonUtility.ToJson(new GameData()));
        
        Dictionary<string, string> data = new Dictionary<string, string> { { "DisplayName", "" }, {"GameDataKey", ""} };

        data["DisplayName"] = _DisplayName;
        data["GameDataKey"] = gameDataKey;

        usersRef.Child(_UserID).SetValueAsync(data);
    }

    public void GetPlayerData(string _userID)
    {
        reference.Child("Users").Child(_userID).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogWarning("Get PlayerData is Cancled");
                return;
            }
            else if (task.IsFaulted)
            {
                Debug.LogWarning("Get PlayerData is Faulted");
                return;
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapShot = task.Result;

                if(snapShot.Value != null)
                {
                    //GetGameData(snapShot.Value);
                    Debug.Log("data is Exist");
                    Debug.Log(snapShot.Child("GameDataKey"));
                    GetGameData(snapShot.Child("GameDataKey").Value as string);
                }
                else
                {
                    AddNewPlayer(_userID, GameManager.Instance.googleManager.GetFirebaseUserName());
                    Debug.LogError("data is not Exist");
                }
            }
        });
    }

    protected void GetGameData(string _GameDataKey)
    {
        reference.Child("GameData").Child(_GameDataKey).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogWarning("Get Player GameData is Cancled");
                return;
            }
            else if (task.IsFaulted)
            {
                Debug.LogWarning("Get Player GameData is Faulted");
                return;
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapShot = task.Result;

                if (snapShot.Value != null)
                {
                    Debug.Log(snapShot.GetRawJsonValue());
                    gameData = JsonUtility.FromJson<GameData>(snapShot.GetRawJsonValue());
                    Debug.Log("GameData is Exist and aync Completed");
                }
                else
                {
                    Debug.LogError("data is not Exist");
                }
            }
        });
    }
}
