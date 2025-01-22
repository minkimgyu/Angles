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
    int _runningMinute; // 광고가 생성되는 시간
    string _saveKeyName; // 광고 세이브 키 이름

    JsonParser _parser;
    DateTime _adShowTime; // 광고 보여주는 시간

    // 인터넷에 연결되지 않아 광고 로드가 불가능한 경우
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
            // 처음 로드가 되지 않았거나 인터넷 연결이 안 되어있는 경우

            DateTime currentTime = ServiceLocater.ReturnAdTimer().CurrentTime;
            if (currentTime.CompareTo(_adShowTime) > 0) return true;
            return false;
        } 
    }

    public string LeftTime
    {
        get
        {
            if (CanLoadAdd == false || CanShowAdd == true) return ""; // 로드가 안 되었거나 인터넷 연결이 안된 경우 return;

            DateTime currentTime = ServiceLocater.ReturnAdTimer().CurrentTime;

            TimeSpan timeSpan = _adShowTime.Subtract(currentTime);
            return $"{timeSpan.Hours}:{timeSpan.Minutes.ToString("D2")}:{timeSpan.Seconds.ToString("D2")}";
        }
    }

    public void ResetAdShowTime()
    {
        DateTime newAdShowTime = ServiceLocater.ReturnAdTimer().CurrentTime;
        _adShowTime = newAdShowTime.AddMinutes(_runningMinute);  // 새롭게 초기화 진행
        Save(_adShowTime); // 초기화 한 데이터를 저장
    }

    public AdHandler(string saveKeyName, int runningMinute)
    {
        _parser = new JsonParser();
        _saveKeyName = saveKeyName;
        _runningMinute = runningMinute;

        if (ServiceLocater.ReturnAdTimer().IsLoadSuccess == false) return;

        if(HaveData(_saveKeyName) == false) // 데이터가 없다면
        {
            _adShowTime = ServiceLocater.ReturnAdTimer().CurrentTime;
            Save(_adShowTime);
        }
        else // 데이터를 가지고 있다면
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
