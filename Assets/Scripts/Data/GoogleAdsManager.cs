using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoogleMobileAds.Api;

public class GoogleAdsManager : MonoBehaviour
{
    private readonly string unitID = "ca-app-pub-7286134024153012/9403667362";
    private readonly string test_unitID = "ca-app-pub-3940256099942544/5224354917";

    private readonly string test_DeviceID = "16C3DA15AF9B24A8B0A12CFF5D913282";
    private readonly string test_PDeviceID = "16C3DA15AF9B24A8B0A12CFF5D913282";

    private RewardedAd videoAd;
    public static bool ShowAd = false;
    
    // Start is called before the first frame update
    void Start()
    {
        MobileAds.Initialize("ca-app-pub-7286134024153012/9403667362");
        videoAd = new RewardedAd(test_unitID);
        
        Handle(videoAd);
        Load();

        RequestInterstitial();
    }

    private void RequestReward()
    {
        videoAd = new RewardedAd(test_unitID);

        Handle(videoAd);

        AdRequest request = new AdRequest.Builder().Build();

        videoAd.LoadAd(request);
    }
    void OnDestroy()
    {
        videoAd.OnAdLoaded -= HandleOnAdLoaded;
        videoAd.OnAdFailedToLoad -= HandleOnAdFailedToLoad;
        videoAd.OnAdFailedToShow -= HandleOnAdFailedToShow;
        videoAd.OnAdOpening -= HandleOnAdOpening;
        videoAd.OnAdClosed -= HandleOnAdClosed;
        videoAd.OnUserEarnedReward -= HandleOnUserEarnedReward;
    }

    private void Handle(RewardedAd videoAd)
    {
        videoAd.OnAdLoaded += HandleOnAdLoaded;
        videoAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        videoAd.OnAdFailedToShow += HandleOnAdFailedToShow;
        videoAd.OnAdOpening += HandleOnAdOpening;
        videoAd.OnAdClosed += HandleOnAdClosed;
        videoAd.OnUserEarnedReward += HandleOnUserEarnedReward;
    }

    private void Load()
    {
        //AdRequest request = new AdRequest.Builder().AddTestDevice(test_DeviceID).AddTestDevice(test_PDeviceID).Build();
        AdRequest request = new AdRequest.Builder().Build();

        videoAd.LoadAd(request);
    }

    public RewardedAd ReloadAd()
    {
        Debug.Log("RewardAd is Reload");
        RewardedAd videoAd = new RewardedAd(test_unitID);
        Handle(videoAd);
        AdRequest request = new AdRequest.Builder().Build();

        videoAd.LoadAd(request);
        return videoAd;
    }

    public void Show()
    {
        StartCoroutine("ShowReWardAd");
    }

    private IEnumerator ShowReWardAd()
    {
        while (!videoAd.IsLoaded())
        {
            yield return null;
        }
        videoAd.Show();
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("RewardAd load Successed");
    }
    public void HandleOnAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        Debug.Log("RewardAd load Failed : " + args.Message);
    }
    public void HandleOnAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        Debug.Log("RewardAd to show Failed : " + args.Message);
    }
    public void HandleOnAdOpening(object sender, EventArgs args)
    {
        Debug.Log("RewardAd is Opened Successed");
    }
    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        Debug.Log("RewardAd is Closed");
        this.videoAd = ReloadAd();
    }
    public void HandleOnUserEarnedReward(object sender, EventArgs args)
    {
        Debug.Log("RewardAd can received Reward");
    }

    private InterstitialAd interstitial;

    private void RequestInterstitial()
    {
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";

        this.interstitial = new InterstitialAd(adUnitId);

        this.interstitial.OnAdLoaded += InterstitialAd_HandleOnAdLoaded;
        this.interstitial.OnAdFailedToLoad += InterstitialAd_HandleOnAdFailedToLoad;
        this.interstitial.OnAdOpening += InterstitialAd_HandleOnAdOpening;
        this.interstitial.OnAdClosed += InterstitialAd_HandleOnAdClosed;
        this.interstitial.OnAdLeavingApplication += InterstitialAd_HandleOnAdLeavingApplication;

        AdRequest request = new AdRequest.Builder().Build();

        this.interstitial.LoadAd(request);
    }

    public void ShowInterstitialAd()
    {
        if (this.interstitial.IsLoaded())
        {
            this.interstitial.Show();
        }
    }

    public void InterstitialAd_HandleOnAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("InterstitialAd_ load Successed");
    }
    public void InterstitialAd_HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("InterstitialAd_ load Failed : " + args.Message);
    }
    public void InterstitialAd_HandleOnAdOpening(object sender, EventArgs args)
    {
        Debug.Log("InterstitialAd_ is Opened Successed");
    }
    public void InterstitialAd_HandleOnAdClosed(object sender, EventArgs args)
    {
        RequestInterstitial();
        GameManager.Instance.lakiaroManager.StartGame();
        Debug.Log("InterstitialAd_ is Closed");
    }
    public void InterstitialAd_HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        Debug.Log("InterstitialAd_ can received Reward");
    }
}
