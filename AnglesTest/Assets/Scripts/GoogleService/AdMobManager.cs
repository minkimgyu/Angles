using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;
using System;

public interface IAdMob
{
    virtual void Initialize(Action OnComplete) { }
    // 리워드 콜백으로 보상형 광고 표시
    virtual void ShowRewardedAd(Action OnSuccess, Action OnError) { }
    virtual bool CanShowAdd() {  return false; }
}

public class NullAdMobManager : IAdMob
{ 
}

// 광고 구조 바꿔보기
// 생성자에서 미리 광고 로드
// 광고 플레이 시 광고를 추가로 로드해주기 --> 로드 너무 오래 걸림
// 광고 꺼짐 이벤트로 리워드 수령하지 못한 경우 처리해주기

public class AdMobManager : MonoBehaviour, IAdMob
{
    [SerializeField] bool _isTestMode = true;

    public void Initialize(Action OnComplete)
    {
        DontDestroyOnLoad(gameObject);

//#if UNITY_EDITOR
//        _isTestMode = true; // 에디터의 경우
//#else
//        _isTestMode = false; //  안드로이드의 경우
//#endif

        // Google Mobile Ads SDK 초기화
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // SDK 초기화가 완료된 후 호출되는 콜백
            RequestConfiguration requestConfiguration = new RequestConfiguration();
            requestConfiguration.TestDeviceIds.Add("a169087db8c74d6f"); // s23fe
            requestConfiguration.TestDeviceIds.Add("3d0c8e6bde24e4a3"); // s6lite
            requestConfiguration.TestDeviceIds.Add("429d991807e82f44"); // ha

            MobileAds.SetRequestConfiguration(requestConfiguration);

            _lock = new object();
            _eventQueue = new Queue<Action>();
            LoadRewardedAd(OnComplete);
        });
    }

    object _lock;
    Queue<Action> _eventQueue;

    void Update()
    {
        if (_eventQueue == null || _eventQueue.Count == 0) return;

        Action eventAction = _eventQueue.Dequeue();
        eventAction?.Invoke();
    }


    #region 리워드 광고

    const string _rewardTestID = "ca-app-pub-3940256099942544/5224354917";
    const string _rewardID = "ca-app-pub-5276709661960694/3367887647";
    RewardedAd _rewardedAd;

    // 보상형 광고 로드
    /// <summary>
    /// Loads the rewarded ad.
    /// </summary>
    void LoadRewardedAd(Action OnComplete = null)
    {
        // 보상형 광고 정리
        // Clean up the old ad before loading a new one.
        DestroyAd();

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(_isTestMode ? _rewardTestID : _rewardID, adRequest, (RewardedAd ad, LoadAdError error) => {
            // if error is not null, the load request failed.
            if (error != null || ad == null)
            {
                Debug.LogError("Rewarded ad failed to load an ad " +
                                "with error : " + error);

                lock (_lock) _eventQueue.Enqueue(OnComplete); // 큐에 넣어줌 -> 업데이트에서 꺼내서 실행
                return;
            }

            Debug.Log("Rewarded ad loaded with response : "
                        + ad.GetResponseInfo());

            _rewardedAd = ad;
            RegisterReloadHandler(_rewardedAd); // 광고 재로드를 위한 이벤트 연결
            //RegisterEventHandlers(_rewardedAd); // 광고 이벤트 핸들러 연결

            lock (_lock) _eventQueue.Enqueue(OnComplete); // 큐에 넣어줌 -> 업데이트에서 꺼내서 실행
        });
    }

    public bool CanShowAdd()
    {
        return _rewardedAd != null && _rewardedAd.CanShowAd() == true;
    }

    void DestroyAd()
    {
        if (_rewardedAd != null)
        {
            Debug.Log("Destroying rewarded ad.");
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }
    }

    bool _rewardCalled;

    async void CheckRewardCalled(Action OnError)
    {
        await System.Threading.Tasks.Task.Delay(3000);
        if (_rewardCalled == true)
        {
            // 리워드 받아짐
            Debug.Log("리워드 받아짐");
            return;
        }

        Debug.Log("리워드 안 받아짐");
        lock (_lock) _eventQueue.Enqueue(OnError); // 에러를 넣음
    }

    // 리워드 콜백으로 보상형 광고 표시
    public void ShowRewardedAd(Action OnSuccess, Action OnError)
    {
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        _rewardCalled = false;

        if (CanShowAdd() == true)
        {
            // 광고 종료 이후 일정 시간 이후에도 리워드가 들어오지 않는 경우 OnError 리턴
            _rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                CheckRewardCalled(OnError);
            };

            _rewardedAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                _rewardCalled = true;
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
                lock (_lock) _eventQueue.Enqueue(OnSuccess);
            });
        }
        else
        {
            lock (_lock) _eventQueue.Enqueue(OnError);
        }
    }

    //void RegisterEventHandlers(RewardedAd ad)
    //{
    //    // Raised when the ad is estimated to have earned money.
    //    ad.OnAdPaid += (AdValue adValue) =>
    //    {
    //        _rewardCalled = true; // 리워드가 호출됨

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

    void RegisterReloadHandler(RewardedAd ad)
    {
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded Ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();
        };
    }

    #endregion
}
