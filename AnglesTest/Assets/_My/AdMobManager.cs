using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;
using System;
using static InputController;

public interface IAdMob
{
    virtual void LoadRewardedAd(Action OnError) { }

    // 리워드 콜백으로 보상형 광고 표시
    virtual void ShowRewardedAd(Action OnError) { }

    virtual bool CanGetReward { get { return false; } }
    virtual void GetReward() { }

    virtual bool CanLoadAd { get { return false; } }
    virtual void GetAd() { }

    //// 보상형 광고 이벤트 수신
    //virtual void RegisterEventHandlers(RewardedAd ad) { }

    //// 다음 보상형 광고 미리 로드
    //virtual void RegisterReloadHandler(RewardedAd ad) { }
}

public class NULLAdMobManager : IAdMob
{ 
}

public class AdMobManager : IAdMob
{
    public bool _isTestMode = true;
    public Text LogText;
    public Button RewardAdsBtn;

    bool _getReward = false;
    bool _getLoad = false;

    public AdMobManager()
    {
//#if UNITY_ANDROID && UNITY_EDITOR // 에디터의 경우
//        _isTestMode = true;
//#elif UNITY_ANDROID // 안드로이드의 경우
//        _isTestMode = false;
//#endif


        // Google Mobile Ads SDK 초기화
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // SDK 초기화가 완료된 후 호출되는 콜백
        });

        RequestConfiguration requestConfiguration = new RequestConfiguration();
        requestConfiguration.TestDeviceIds.Add("2E25EEE6DF5651D6");

        MobileAds.SetRequestConfiguration(requestConfiguration);
    }

    public bool CanGetReward { get { return _getReward; } }
    public void GetReward() { _getReward = false; }

    public bool CanLoadAd { get { return _getLoad; } }
    public void GetAd() { _getLoad = false; }

    #region 리워드 광고

    const string _rewardTestID = "ca-app-pub-3940256099942544/5224354917";
    const string _rewardID = "ca-app-pub-5276709661960694/3367887647";
    RewardedAd _rewardedAd;

    // 보상형 광고 로드
    /// <summary>
    /// Loads the rewarded ad.
    /// </summary>
    public void LoadRewardedAd(Action OnError)
    {
        // 보상형 광고 정리
        // Clean up the old ad before loading a new one.
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(_isTestMode ? _rewardTestID : _rewardID, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);

                    OnError?.Invoke();
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                _rewardedAd = ad;
                _getLoad = true;
            });
    }

    // 리워드 콜백으로 보상형 광고 표시
    public void ShowRewardedAd(Action OnError)
    {
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (_rewardedAd != null && _rewardedAd.CanShowAd() == true)
        {
            _rewardedAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));

                _getReward = true;
            });
        }
        else
        {
            OnError?.Invoke();
        }
    }

    #endregion
}
