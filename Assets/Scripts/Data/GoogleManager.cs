using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System.Threading.Tasks;

using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Database;
using Firebase.Unity.Editor;

public class GoogleManager : MonoBehaviour
{
    public GameObject btn;
    private string authCode;
    Firebase.Auth.FirebaseAuth auth;

    // Start is called before the first frame update
    void Start()
    {
        //CheckAndFix();
        ActivePlayGame();
    }

    public void ActivePlayGame()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().RequestServerAuthCode(false).Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        
        GooglePlayGameLogin();
    }

    public void GooglePlayGameLogin()
    {
        Social.localUser.Authenticate((result, errorMessage) =>
        {
            if (result)
            {
                authCode = PlayGamesPlatform.Instance.GetServerAuthCode();
                btn.SetActive(true);
                Debug.LogWarning("구글 플레이 로그인 성공");

                PlayGamesAuthFirebase(authCode);
            }
            else
            {
                Debug.LogWarning("구글 플레이 로그인 실패 익명 로그인 시작 :" + errorMessage.ToString());
                AnonymouslyLogin();
            }
        });
    }

    public void AnonymouslyLogin()
    {
        Debug.Log("익명 로그인 시작");
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        Task tasks = auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("익명 로그인 취소됨");
                return;
            }
            if (task.IsFaulted)
            {
            Debug.Log("익명 로그인 실패");
                return;
            }

            FirebaseUser user = task.Result;
            Debug.LogFormat("사용자가 성공적으로 로그인함 : {0}  ({1})", user.DisplayName, user.UserId);

            btn.SetActive(true);
            GameManager.Instance.dataManager.StartFirebase();
        });
    }

    public void LogOut()
    {
        PlayGamesPlatform.Instance.SignOut();
    }

    public void PlayGamesAuthFirebase(string _authCode)
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        Firebase.Auth.Credential credential = Firebase.Auth.PlayGamesAuthProvider.GetCredential(authCode);
        auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync 취소됨");
                return;
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync 에러 : " + task.Exception);
                return;
            }

            FirebaseUser user = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", user.DisplayName, user.UserId);
            GameManager.Instance.dataManager.StartFirebase();
        });
    }

    public string GetFirebaseUserID()
    {
        Firebase.Auth.FirebaseUser user = Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser;
        string uid = null;
        if (user != null)
        {
            uid = user.UserId;
        }
        return uid;
    }

    public string GetFirebaseUserName()
    {
        Firebase.Auth.FirebaseUser user = Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser;
        string name = null;
        if (user != null)
        {
            name = user.DisplayName;
        }
        return name;
    }

    public void FirebaseLogOut()
    {
        FirebaseAuth.DefaultInstance.SignOut();
    }

    //임시 형성
    Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;

    public void CheckAndFix()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            dependencyStatus = task.Result;

            if(dependencyStatus == Firebase.DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError(dependencyStatus.ToString());
            }
        });
    }

    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    private void InitializeFirebase()
    {
        Debug.Log("파이어베이스 Auth Setting up");
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    FirebaseUser user;

    private void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if(auth.CurrentUser != user)
        {
            Debug.Log("auth.CurrentUser != user");
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if(!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.LogFormat("Signed in : <color='red'>{0}</color>", user.UserId);
            }
        }
        else
        {
            //this.ClickLogin();
        }
    }
}
