using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public interface IAdMob
{
    virtual void Initialize(Action OnComplete) { }
    // ������ �ݹ����� ������ ���� ǥ��

    virtual void ShowAd(Action OnSuccess, Action OnError) { }
    //virtual void ShowRewardedAd(Action OnSuccess, Action OnError) { }
    virtual bool CanShowAd() {  return false; }
}

public class NullAdMobManager : IAdMob
{ 
}

// ���� ���� �ٲ㺸��
// �����ڿ��� �̸� ���� �ε�
// ���� �÷��� �� ���� �߰��� �ε����ֱ� --> �ε� �ʹ� ���� �ɸ�
// ���� ���� �̺�Ʈ�� ������ �������� ���� ��� ó�����ֱ�

public class AdMobManager : MonoBehaviour, IAdMob
{
    bool _isTestMode = true;
    bool _canLoadAd = true;

    public void Initialize(Action OnComplete)
    {
        DontDestroyOnLoad(gameObject);

#if UNITY_EDITOR
        _isTestMode = true; // �������� ���
#elif UNITY_ANDROID
        _isTestMode = false; //  �ȵ���̵��� ���
#endif

        // Google Mobile Ads SDK �ʱ�ȭ
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // SDK �ʱ�ȭ�� �Ϸ�� �� ȣ��Ǵ� �ݹ�
            //RequestConfiguration requestConfiguration = new RequestConfiguration();
            //requestConfiguration.TestDeviceIds.Add("a169087db8c74d6f"); // s23fe
            //requestConfiguration.TestDeviceIds.Add("3d0c8e6bde24e4a3"); // s6lite
            //requestConfiguration.TestDeviceIds.Add("429d991807e82f44"); // ha
            // ha�� �º��� �־���´�.

            //MobileAds.SetRequestConfiguration(requestConfiguration);

            _lock = new object();
            _eventQueue = new Queue<Action>();
            lock (_lock) _eventQueue.Enqueue(OnComplete); // ť�� �־��� -> ������Ʈ���� ������ ����
            //LoadRewardedAd(OnComplete);
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


    #region ������ ����

    const string _rewardTestID = "ca-app-pub-3940256099942544/5224354917";
    const string _rewardID = "ca-app-pub-5276709661960694/3367887647";
    RewardedAd _rewardedAd;

    public void ShowAd(Action OnSuccess, Action OnError)
    {
        bool canShowAd = CanShowAd();

        if (canShowAd == false) // ���� �� �� ���ٸ�
        {
            LoadRewardedAd(() => // ���� �ε��ϰ� �����Ѵٸ� ���� �����ֱ�
            {
                ShowRewardedAd(() =>
                {
                    OnSuccess?.Invoke();  // ���� ��� ���� �� ���� ȣ��
                },
                () =>
                {
                    OnError?.Invoke(); // ���� ��� ���� �� ���� ȣ��
                });
            }, () =>
            {
                OnError?.Invoke(); // ���� �ε� ���� �� ���� ȣ��
            });
        }
        else
        {
            ShowRewardedAd(() =>
            {
                OnSuccess?.Invoke();  // ���� ��� ���� �� ���� ȣ��
            },
            () =>
            {
                OnError?.Invoke(); // ���� ��� ���� �� ���� ȣ��
            });
        }
    }


    // ������ ���� �ε�
    /// <summary>
    /// Loads the rewarded ad.
    /// </summary>
    void LoadRewardedAd(Action OnComplete, Action OnError)
    {
        if(_canLoadAd == false) // ���� �ε��� �� ������ �ε����� �ʴ´�.
        {
            Debug.Log("dont load ad");
            lock (_lock) _eventQueue.Enqueue(OnError); // ť�� �־��� -> ������Ʈ���� ������ ����
            return;
        }

        // ������ ���� ����
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

                lock (_lock) _eventQueue.Enqueue(OnError); // ť�� �־��� -> ������Ʈ���� ������ ����
                return;
            }

            Debug.Log("Rewarded ad loaded with response : "
                        + ad.GetResponseInfo());

            _rewardedAd = ad;
            RegisterReloadHandler(_rewardedAd); // ���� ��ε带 ���� �̺�Ʈ ����
            //RegisterEventHandlers(_rewardedAd); // ���� �̺�Ʈ �ڵ鷯 ����

            lock (_lock) _eventQueue.Enqueue(OnComplete); // ť�� �־��� -> ������Ʈ���� ������ ����
        });
    }

    public bool CanShowAd()
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
    const int _delay = 3000;

    async void CheckRewardCalled(Action OnError)
    {
        await System.Threading.Tasks.Task.Delay(_delay);
        if (_rewardCalled == true)
        {
            // ������ �޾���
            Debug.Log("������ �޾���");
            return;
        }

        Debug.Log("������ �� �޾���");
        lock (_lock) _eventQueue.Enqueue(OnError); // ������ ����
    }

    // ������ �ݹ����� ������ ���� ǥ��
    void ShowRewardedAd(Action OnSuccess, Action OnError)
    {
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        _rewardCalled = false;

        // ���� ���� ���� ���� �ð� ���Ŀ��� �����尡 ������ �ʴ� ��� OnError ����
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

        //if (CanShowAd() == true)
        //{
            
        //}
        //else
        //{
        //    lock (_lock) _eventQueue.Enqueue(OnError);
        //}
    }

    //void RegisterEventHandlers(RewardedAd ad)
    //{
    //    // Raised when the ad is estimated to have earned money.
    //    ad.OnAdPaid += (AdValue adValue) =>
    //    {
    //        _rewardCalled = true; // �����尡 ȣ���

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
            LoadRewardedAd(null, null);
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd(null, null);
        };
    }

    #endregion
}
