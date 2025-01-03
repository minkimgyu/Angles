using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

public interface ISaveable
{
    void Save();
    void ClearSave();

    void Load();

    void ChangeBGMMute(bool nowMute);
    void ChangeSFXMute(bool nowMute);

    void ChangeLanguage(ILocalization.Language language);

    void ChangeSkin(SkinData.Key name);
    void UnlockSkin(SkinData.Key name);

    void ChangeStat(StatData.Key name, int level);

    void ChangeLevelProgress(GameMode.Level level, int completeLevel);
    void ChangeLevelDuration(GameMode.Level level, int completeDuration);
    void ChangeCurrentLevel(GameMode.Type type, GameMode.Level level);

    /// <summary>
    /// 코인을 추가한다.
    /// </summary>
    void AddCoinCount(int cointCount);

    /// <summary>
    /// 코인 개수를 대입해준다.
    /// </summary>
    void ChangeCoinCount(int cointCount);

    void UnlockLevel(GameMode.Type type, GameMode.Level level);
    SaveData GetSaveData() { return default; }
}

public class NULLSaveManager : ISaveable
{
    public void Load() { }
    public void Save() { }
    public SaveData GetSaveData() { return default; }



    public void AddCoinCount(int cointCount) { }
    public void ChangeCoinCount(int cointCount) { }

    public void UnlockLevel(GameMode.Type type, GameMode.Level level) { }
    public void ChangeStat(StatData.Key name, int level) { }

    public void ChangeSkin(SkinData.Key name) { }
    public void UnlockSkin(SkinData.Key name) { }


    public void ChangeBGMMute(bool nowMute) { }
    public void ChangeSFXMute(bool nowMute) { }

    public void ClearSave() { }

    public void ChangeLevelProgress(GameMode.Level level, int completeLevel) { }
    public void ChangeLevelDuration(GameMode.Level level, int completeDuration) { }
    public void ChangeCurrentLevel(GameMode.Type type, GameMode.Level level) { }

    public void ChangeLanguage(ILocalization.Language language) { }
}

public struct SaveData
{
    public bool _muteBGM;
    public bool _muteSFX;

    [JsonConverter(typeof(StringEnumConverter))] public ILocalization.Language _language;

    public int _gold; // 골드
    public Dictionary<GameMode.Type, GameMode.Level> _selectedLevel; // 선택된 스테이지 정보

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
        _selectedLevel[GameMode.Type.Chapter] = GameMode.Level.TriconChapter;
        _selectedLevel[GameMode.Type.Survival] = GameMode.Level.PyramidSurvival;

        List<GameMode.Level> chapterLevels = GameMode.GetLevels(GameMode.Type.Chapter);
        for (int i = 0; i < chapterLevels.Count; i++)
        {
            if (_levelInfos.ContainsKey(chapterLevels[i]) == true) continue;

            _levelInfos[chapterLevels[i]] = new SavableChapterInfo(0, false);
        }

        List<GameMode.Level> survivalLevels = GameMode.GetLevels(GameMode.Type.Survival);
        for (int i = 0; i < survivalLevels.Count; i++)
        {
            if (_levelInfos.ContainsKey(survivalLevels[i]) == true) continue;

            _levelInfos[survivalLevels[i]] = new SavableSurvivalInfo(0, false);
        }

        foreach (StatData.Key key in Enum.GetValues(typeof(StatData.Key)))
        {
            if(_statInfos.ContainsKey(key) == false) _statInfos.Add(key, new SavableStatData(0));
        }

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

    public void ChangeCurrentLevel(GameMode.Type type, GameMode.Level level)
    {
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

    public void UnlockLevel(GameMode.Type type, GameMode.Level level) 
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
        if (File.Exists(_filePath))
        {
            _saveData = _defaultSaveData;
            Save();
            return;
        }
    }

    public void Load()
    {
        if (!File.Exists(_filePath))
        {
            _saveData = _defaultSaveData;
            Save();
            return;
        }

        string json = File.ReadAllText(_filePath);

        // 불러오는 중 오류가 있다면 기본 세이브로 바꾸고 다시 저장
        try
        {
            _saveData = _parser.JsonToObject<SaveData>(json);
            _saveData.Update(); // 저장한 데이터를 로드할 때 업데이트 필요
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            _saveData = _defaultSaveData; // 기본적으로 업데이트 되어있음
            Save();
        }
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
