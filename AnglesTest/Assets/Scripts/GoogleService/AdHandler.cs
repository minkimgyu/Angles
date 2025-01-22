using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using Newtonsoft.Json;

public struct AdData
{
    [JsonProperty] int _lobbyAdCoinCount;
    [JsonIgnore] public int LobbyAdCoinCount { get { return _lobbyAdCoinCount; } }


    [JsonProperty] string _lobbyAdSaveKeyName;
    [JsonIgnore] public string LobbyAdSaveKeyName { get { return _lobbyAdSaveKeyName; } }


    [JsonProperty] int _lobbyAdDelay;
    [JsonIgnore] public int LobbyAdDelay { get { return _lobbyAdDelay; } }


    [JsonProperty] string _inGameAdSaveKeyName;
    [JsonIgnore] public string InGameAdSaveKeyName { get { return _inGameAdSaveKeyName; } }


    [JsonProperty] int _inGameAdDelay;
    [JsonIgnore] public int InGameAdDelay { get { return _inGameAdDelay; } }


    public AdData(
        int lobbyAdCoinCount,
        string lobbyAdSaveKeyName,
        int lobbyAdDelay,
        string inGameAdSaveKeyName,
        int inGameAdDelay)
    {
        _lobbyAdCoinCount = lobbyAdCoinCount;
        _lobbyAdSaveKeyName = lobbyAdSaveKeyName;
        _lobbyAdDelay = lobbyAdDelay;
        _inGameAdSaveKeyName = inGameAdSaveKeyName;
        _inGameAdDelay = inGameAdDelay;
    }
}

public class AdHandler
{
    int _runningMinute; // ���� �����Ǵ� �ð�
    string _saveKeyName; // ���� ���̺� Ű �̸�

    JsonParser _parser;
    DateTime _adShowTime; // ���� �����ִ� �ð�

    // ���ͳݿ� ������� �ʾ� ���� �ε尡 �Ұ����� ���
    bool CanLoadAdd
    {
        get
        {
            if (ServiceLocater.ReturnAdTimer().IsLoadSuccess == false ||
                Application.internetReachability == NetworkReachability.NotReachable) return false;

            return true;
        }
    }

    public bool CanShowAdd
    {
        get
        {
            if (CanLoadAdd == false) return false;
            // ó�� �ε尡 ���� �ʾҰų� ���ͳ� ������ �� �Ǿ��ִ� ���

            DateTime currentTime = ServiceLocater.ReturnAdTimer().CurrentTime;
            if (currentTime.CompareTo(_adShowTime) > 0) return true;
            return false;
        } 
    }

    public string LeftTime
    {
        get
        {
            if (CanLoadAdd == false || CanShowAdd == true) return ""; // �ε尡 �� �Ǿ��ų� ���ͳ� ������ �ȵ� ��� return;

            DateTime currentTime = ServiceLocater.ReturnAdTimer().CurrentTime;

            TimeSpan timeSpan = _adShowTime.Subtract(currentTime);
            return $"{timeSpan.Hours}:{timeSpan.Minutes.ToString("D2")}:{timeSpan.Seconds.ToString("D2")}";
        }
    }

    public void ResetAdShowTime()
    {
        DateTime newAdShowTime = ServiceLocater.ReturnAdTimer().CurrentTime;
        _adShowTime = newAdShowTime.AddMinutes(_runningMinute);  // ���Ӱ� �ʱ�ȭ ����
        Save(_adShowTime); // �ʱ�ȭ �� �����͸� ����
    }

    public AdHandler(string saveKeyName, int runningMinute)
    {
        _parser = new JsonParser();
        _saveKeyName = saveKeyName;
        _runningMinute = runningMinute;

        if (ServiceLocater.ReturnAdTimer().IsLoadSuccess == false) return;

        if(HaveData(_saveKeyName) == false) // �����Ͱ� ���ٸ�
        {
            _adShowTime = ServiceLocater.ReturnAdTimer().CurrentTime;
            Save(_adShowTime);
        }
        else // �����͸� ������ �ִٸ�
        {
            _adShowTime = Load(_saveKeyName);
        }
    }
   

    DateTime Load(string key)
    {
        string jData = PlayerPrefs.GetString(key);
        return _parser.JsonToObject<DateTime>(jData);
    }

    void Save(DateTime time)
    {
        string jData = _parser.ObjectToJson(time);
        Debug.Log(jData);
        PlayerPrefs.SetString(_saveKeyName, jData);
    }

    bool HaveData(string key)
    {
        return PlayerPrefs.HasKey(key);
    }
}
