using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

public interface ISaveable
{
    virtual bool VerifyJson(string json) { return false; }
    virtual bool VerifyJson(string json, out SaveData saveData) { saveData = default; return default; }

    virtual bool HaveSaveFile() { return false; }

    virtual void Save(string saveJson) { }
    virtual void Save() { }
    virtual void ClearSave() { }

    virtual void Load() { }

    virtual void ChangeBGMMute(bool nowMute) { }
    virtual void ChangeSFXMute(bool nowMute) { }

    virtual void ChangeLanguage(ILocalization.Language language) { }

    virtual void ChangeType(GameMode.Type type) { }
    virtual void ChangeSkin(SkinData.Key name) { }
    virtual void UnlockSkin(SkinData.Key name) { }

    virtual void ChangeStat(StatData.Key name, int level) { }

    virtual void ChangeLevelProgress(GameMode.Level level, int completeLevel) { }
    virtual void ChangeLevelDuration(GameMode.Level level, int completeDuration) { }
    virtual void ChangeCurrentLevel(GameMode.Level level) { }

    /// <summary>
    /// 코인을 추가한다.
    /// </summary>
    virtual void AddCoinCount(int cointCount) { }

    /// <summary>
    /// 코인 개수를 대입해준다.
    /// </summary>
    virtual void ChangeCoinCount(int cointCount) { }

    virtual void UnlockLevel(GameMode.Level level) { }

    /// <summary>
    /// 세이브 데이터 가져오기
    /// </summary>
    /// <returns></returns>
    virtual SaveData GetSaveData() { return default; }

    /// <summary>
    /// 세이브 Json 데이터 가져오기
    /// </summary>
    /// <returns></returns>
    virtual string GetSaveJsonData() { return default; }
}

public class NULLSaveManager : ISaveable { }

public struct SaveData
{
    public bool _muteBGM;
    public bool _muteSFX;

    [JsonConverter(typeof(StringEnumConverter))] public ILocalization.Language _language;
    [JsonConverter(typeof(StringEnumConverter))] public GameMode.Type _selectedType;

    public int _gold; // 골드

    [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
    public Dictionary<GameMode.Type, GameMode.Level> _selectedLevel; // 타입별 선택된 스테이지 정보

    [JsonConverter(typeof(StringEnumConverter))] public SkinData.Key _skin;
    public Dictionary<GameMode.Level, ISavableLevelInfo> _levelInfos; // 레벨 정보
    public Dictionary<StatData.Key, SavableStatData> _statInfos; // 스텟 레벨 정보
    public Dictionary<SkinData.Key, SavableSkinData> _skinInfos; // 스킨 해금 정보

    // 딕셔너리를 채워주는 코드 필요
    // --> 새로 생긴 데이터를 확인하는 과정에서 문제가 생김
    // 해결 방법 --> 만약 딕셔너리에 없다면 해당 데이터를 그때 채워넣는다.
    // --> 좀 더 생각해보자

    public SaveData(int gold, bool muteBGM = false, bool muteSFX = false)
    {
        _muteBGM = muteBGM;
        _muteSFX = muteSFX;
        _gold = gold;

        _selectedType = GameMode.Type.Chapter;
        _language = ILocalization.Language.Korean;

        _selectedLevel = new Dictionary<GameMode.Type, GameMode.Level>();
        _selectedLevel[GameMode.Type.Chapter] = GameMode.Level.TriconChapter;
        _selectedLevel[GameMode.Type.Survival] = GameMode.Level.PyramidSurvival;

        _skin = SkinData.Key.Normal;

        _levelInfos = new Dictionary<GameMode.Level, ISavableLevelInfo>();

        List<GameMode.Level> chapterLevels = GameMode.GetLevels(GameMode.Type.Chapter);
        for (int i = 0; i < chapterLevels.Count; i++)
        {
            if (i == 0) _levelInfos[chapterLevels[i]] = new SavableChapterInfo(0, true);
            else _levelInfos[chapterLevels[i]] = new SavableChapterInfo(0, false);
        }

        List<GameMode.Level> survivalLevels = GameMode.GetLevels(GameMode.Type.Survival);
        for (int i = 0; i < survivalLevels.Count; i++)
        {
            if (i == 0) _levelInfos[survivalLevels[i]] = new SavableSurvivalInfo(0, true);
            else _levelInfos[survivalLevels[i]] = new SavableSurvivalInfo(0, false);
        }

        _statInfos = new Dictionary<StatData.Key, SavableStatData>();
        foreach (StatData.Key i in Enum.GetValues(typeof(StatData.Key)))
        {
            _statInfos.Add(i, new SavableStatData(0));
        }

        _skinInfos = new Dictionary<SkinData.Key, SavableSkinData>();
        foreach (SkinData.Key i in Enum.GetValues(typeof(SkinData.Key)))
        {
            if (i == 0) _skinInfos.Add(i, new SavableSkinData(true));
            else _skinInfos.Add(i, new SavableSkinData(false));
        }
    }

    // Json 데이터 업데이트
    public void Update()
    {
        // 만약 Chapter 키에 담긴 레벨이 없다면 진행
        if (_selectedLevel.ContainsKey(GameMode.Type.Chapter) == false)
        {
            _selectedLevel[GameMode.Type.Chapter] = GameMode.Level.TriconChapter;
        }

        // 만약 Survival 키에 담긴 레벨이 없다면 진행
        if (_selectedLevel.ContainsKey(GameMode.Type.Survival) == false)
        {
            _selectedLevel[GameMode.Type.Survival] = GameMode.Level.PyramidSurvival;
        }

        // 새로 추가된 데이터를 기본 데이터로 추가해준다.
        List<GameMode.Level> chapterLevels = GameMode.GetLevels(GameMode.Type.Chapter);
        for (int i = 0; i < chapterLevels.Count; i++)
        {
            if (_levelInfos.ContainsKey(chapterLevels[i]) == true) continue;

            _levelInfos[chapterLevels[i]] = new SavableChapterInfo(0, false);
        }

        // 새로 추가된 데이터를 기본 데이터로 추가해준다.
        List<GameMode.Level> survivalLevels = GameMode.GetLevels(GameMode.Type.Survival);
        for (int i = 0; i < survivalLevels.Count; i++)
        {
            if (_levelInfos.ContainsKey(survivalLevels[i]) == true) continue;

            _levelInfos[survivalLevels[i]] = new SavableSurvivalInfo(0, false);
        }

        // 새로 추가된 데이터를 기본 데이터로 추가해준다.
        foreach (StatData.Key key in Enum.GetValues(typeof(StatData.Key)))
        {
            if(_statInfos.ContainsKey(key) == false) _statInfos.Add(key, new SavableStatData(0));
        }

        // 새로 추가된 데이터를 기본 데이터로 추가해준다.
        foreach (SkinData.Key key in Enum.GetValues(typeof(SkinData.Key)))
        {
            if (_skinInfos.ContainsKey(key) == false) _skinInfos.Add(key, new SavableSkinData(false));
        }
    }
}

public class SaveManager : ISaveable
{
    JsonParser _parser;
    string _filePath;

    SaveData _defaultSaveData;
    SaveData _saveData;

    public SaveManager(SaveData defaultSaveData)
    {
        _parser = new JsonParser();
        _defaultSaveData = defaultSaveData;
        _filePath = Application.persistentDataPath + "/SaveData.txt";
        Debug.Log(_filePath);
        Load();
    }

    public void ChangeLanguage(ILocalization.Language language) 
    { 
        _saveData._language = language;
        Save();
    }

    public void ChangeSkin(SkinData.Key name)
    {
        _saveData._skin = name;
        Save();
    }

    public void UnlockSkin(SkinData.Key name)
    {
        SavableSkinData savableSkinData = _saveData._skinInfos[name];
        savableSkinData._nowUnlock = true;

        _saveData._skinInfos[name] = savableSkinData;
        Save();
    }

    public void ChangeStat(StatData.Key name, int level)
    {
        SavableStatData savableStatData = _saveData._statInfos[name];
        savableStatData._currentLevel = level;

        _saveData._statInfos[name] = savableStatData;
        Save();
    }

    public void ChangeLevelProgress(GameMode.Level level, int completeLevel)
    {
        ISavableLevelInfo savableChapterInfo = _saveData._levelInfos[level];
        savableChapterInfo.CompleteLevel = completeLevel;

        _saveData._levelInfos[level] = savableChapterInfo;
        Save();
    }

    public void ChangeLevelDuration(GameMode.Level level, int completeDuration)
    {
        ISavableLevelInfo savableChapterInfo = _saveData._levelInfos[level];
        savableChapterInfo.CompleteDuration = completeDuration;

        _saveData._levelInfos[level] = savableChapterInfo;
        Save();
    }

    public void ChangeCurrentLevel(GameMode.Level level)
    {
        GameMode.Type type = GameMode.GetLevelType(level);
        _saveData._selectedLevel[type] = level;
        Save();
    }
   
    public void AddCoinCount(int cointCount)
    {
        _saveData._gold += cointCount;
        Save();
    }

    public void ChangeCoinCount(int cointCount) 
    {
        _saveData._gold = cointCount;
        Save();
    }

    public void ChangeType(GameMode.Type type)
    {
        _saveData._selectedType = type;
        Save();
    }

    public void UnlockLevel(GameMode.Level level) 
    {
        ISavableLevelInfo savableChapterInfo = _saveData._levelInfos[level];
        savableChapterInfo.NowUnlock = true;

        _saveData._levelInfos[level] = savableChapterInfo;
        Save();
    }

    public SaveData GetSaveData()
    {
        return _saveData;
    }

    public void ClearSave() 
    {
        if (HaveSaveFile())
        {
            _saveData = _defaultSaveData;
            Save();
            return;
        }
    }

    public string GetSaveJsonData()
    {
        // 파일이 존재하지 않는다면
        if (!HaveSaveFile())
        {
            _saveData = _defaultSaveData;
            Save(); // 세이브 파일을 만들어주고 저장한다.
        }

        // 저장된 파일을 불러서 리턴한다.
        string json = File.ReadAllText(_filePath);
        return json;
    }


    /// <summary>
    /// GPGS용 데이터 검증
    /// 만약 서버에서 받은 데이터가 고장난 경우 기존 데이터를 삭제하지 않고
    /// 그대로 사용
    /// </summary>

    public bool VerifyJson(string json)
    {
        // 불러오는 중 오류가 있다면 더 이상 진행하지 않음
        try
        {
            SaveData newSaveData = _parser.JsonToObject<SaveData>(json);
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            return false; // 유효하지 않음
        }

        return true; // 유효함
    }


    public bool VerifyJson(string json, out SaveData saveData)
    {
        // 불러오는 중 오류가 있다면 더 이상 진행하지 않음
        try
        {
            saveData = _parser.JsonToObject<SaveData>(json);
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            saveData = default;
            return false; // 유효하지 않음
        }

        return true; // 유효함
    }

    public bool HaveSaveFile()
    {
        return File.Exists(_filePath);
    }

    public void Load()
    {
        // 파일이 존재하지 않는다면
        if (!HaveSaveFile())
        {
            _saveData = _defaultSaveData; // 기본 세이브로 대체해준다.
            Save();
            return;
        }

        string json = File.ReadAllText(_filePath);

        SaveData newSaveData;
        bool nowValidate = VerifyJson(json, out newSaveData);

        if(nowValidate == true)
        {
            _saveData = newSaveData; // 유효하다면 해당 데이터를 사용한다.
            _saveData.Update(); // 기존 데이터를 업데이트 해준다.
        }
        else
        {
            _saveData = _defaultSaveData; // 기본 데이터로 대체한다.
            Save();
        }
    }

    /// <summary>
    /// GPGS용 세이브
    /// </summary>
    public void Save(string saveJson)
    {
        bool nowValidate = VerifyJson(saveJson);
        if (nowValidate == false) return; // 유효하지 않다면 저장하지 않음

        File.WriteAllText(_filePath, saveJson);
    }

    public void Save()
    {
        string json = _parser.ObjectToJson(_saveData);
        File.WriteAllText(_filePath, json);
    }

    public void ChangeBGMMute(bool nowMute)
    {
        _saveData._muteBGM = nowMute;
        Save();
    }

    public void ChangeSFXMute(bool nowMute)
    {
        _saveData._muteSFX = nowMute;
        Save();
    }
}
