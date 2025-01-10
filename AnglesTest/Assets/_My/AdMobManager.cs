using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;
using System;

public interface IAdMob
{
    virtual void LoadRewardedAd(Action OnError) { }

    // ������ �ݹ����� ������ ���� ǥ��
    virtual void ShowRewardedAd(Action OnError) { }

    virtual bool CanGetReward { get { return false; } }
    virtual void GetReward() { }

    virtual bool CanLoadAd { get { return false; } }
    virtual void GetAd() { }

    //// ������ ���� �̺�Ʈ ����
    //virtual void RegisterEventHandlers(RewardedAd ad) { }

    //// ���� ������ ���� �̸� �ε�
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
        // Google Mobile Ads SDK �ʱ�ȭ
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // SDK �ʱ�ȭ�� �Ϸ�� �� ȣ��Ǵ� �ݹ�
        });

        RequestConfiguration requestConfiguration = new RequestConfiguration();
        requestConfiguration.TestDeviceIds.Add("2E25EEE6DF5651D6");

        MobileAds.SetRequestConfiguration(requestConfiguration);
    }

    public bool CanGetReward { get { return _getReward; } }
    public void GetReward() { _getReward = false; }

    public bool CanLoadAd { get { return _getLoad; } }
    public void GetAd() { _getLoad = false; }

    #region ������ ����

    const string _rewardTestID = "ca-app-pub-3940256099942544/5224354917";
    const string _rewardID = "ca-app-pub-5276709661960694/3367887647";
    RewardedAd _rewardedAd;

    // ������ ���� �ε�
    /// <summary>
    /// Loads the rewarded ad.
    /// </summary>
    public void LoadRewardedAd(Action OnError)
    {
        // ������ ���� ����
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

    // ������ �ݹ����� ������ ���� ǥ��
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

    //// ������ ���� �̺�Ʈ ����
    //public void RegisterEventHandlers(RewardedAd ad)
    //{
    //    // Raised when the ad is estimated to have earned money.
    //    ad.OnAdPaid += (AdValue adValue) =>
    //    {
    //        Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
    //            adValue.Value,
    //            adValue.CurrencyCode));
    //    };
    //    // Raised when an impression is recorded for an ad.
    //    ad.OnAdImpressionRecorded += () =>
    //    {
    //        Debug.Log("Rewarded ad recorded an impression.");
    //    };
    //    // Raised when a click is recorded for an ad.
    //    ad.OnAdClicked += () =>
    //    {
    //        Debug.Log("Rewarded ad was clicked.");
    //    };
    //    // Raised when an ad opened full screen content.
    //    ad.OnAdFullScreenContentOpened += () =>
    //    {
    //        Debug.Log("Rewarded ad full screen content opened.");
    //    };
    //    // Raised when the ad closed full screen content.
    //    ad.OnAdFullScreenContentClosed += () =>
    //    {
    //        Debug.Log("Rewarded ad full screen content closed.");
    //    };
    //    // Raised when the ad failed to open full screen content.
    //    ad.OnAdFullScreenContentFailed += (AdError error) =>
    //    {
    //        Debug.LogError("Rewarded ad failed to open full screen content " +
    //                       "with error : " + error);
    //    };
    //}

    // ���� ������ ���� �̸� �ε�
    //public void RegisterReloadHandler(RewardedAd ad)
    //{
    //    // Raised when the ad closed full screen content.
    //    ad.OnAdFullScreenContentClosed += () =>
    //    {
    //        Debug.Log("Rewarded Ad full screen content closed.");

    //        // Reload the ad so that we can show another as soon as possible.
    //        LoadRewardedAd();
    //    };

    //    // Raised when the ad failed to open full screen content.
    //    ad.OnAdFullScreenContentFailed += (AdError error) =>
    //    {
    //        Debug.LogError("Rewarded ad failed to open full screen content " +
    //                       "with error : " + error);

    //        // Reload the ad so that we can show another as soon as possible.
    //        LoadRewardedAd();
    //    };
    //}

    #endregion
}
