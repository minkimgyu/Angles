using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public interface IAdTimer
{
    virtual void Initialize(Action OnComplete) { }

    virtual bool IsLoadSuccess { get { return false; } } // ���� ����
    virtual DateTime CurrentTime { get { return default; } } // ���� �ð�
}

public class NullAdTimer : IAdTimer
{
}

public class AdTimer : MonoBehaviour, IAdTimer
{
    const string URL = "www.google.com";

    bool _loadSuccess = false;
    public bool IsLoadSuccess { get { return _loadSuccess; } }

    DateTime _loadTime; // �������� �ҷ��� �ð�
    public DateTime CurrentTime // ���� �ð�
    {
        get 
        {
            DateTime currentTime = _loadTime;
            return currentTime.AddSeconds(_passedTime); 
        }
    }

    float _initializedTime = 0; // �� �ʱ� �ð�
    float _passedTime = 0;

    public void Initialize(Action OnComplete)
    {
        DontDestroyOnLoad(gameObject);

        _initializedTime = Time.realtimeSinceStartup;
        StartCoroutine(GetTime(OnSuccess, OnError, OnComplete));
    }

    void Update()
    {
        if (_loadSuccess == false) return;
        _passedTime = Time.realtimeSinceStartup - _initializedTime;
    }

    void OnSuccess(DateTime loadTime)
    {
        _loadTime = loadTime;
        _loadSuccess = true;
    }

    void OnError()
    {
        _loadSuccess = false;
    }

    public IEnumerator GetTime(Action<DateTime> OnSuccess, Action OnError, Action OnComplete)
    {
        UnityWebRequest request = UnityWebRequest.Get(URL);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            OnError?.Invoke();
        }
        else
        {
            string date = request.GetResponseHeader("date");
            DateTime dateTime = DateTime.Parse(date).ToLocalTime();

            Debug.Log(dateTime);
            OnSuccess?.Invoke(dateTime);
        }

        OnComplete?.Invoke();
    }
}
