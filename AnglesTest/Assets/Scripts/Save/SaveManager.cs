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
    /// ������ �߰��Ѵ�.
    /// </summary>
    void AddCoinCount(int cointCount);

    /// <summary>
    /// ���� ������ �������ش�.
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

    public int _gold; // ���
    public Dictionary<GameMode.Type, GameMode.Level> _selectedLevel; // ���õ� �������� ����

    [JsonConverter(typeof(StringEnumConverter))] public SkinData.Key _skin;
    public Dictionary<GameMode.Level, ISavableLevelInfo> _levelInfos; // ���� ����
    public Dictionary<StatData.Key, SavableStatData> _statInfos; // ���� ���� ����
    public Dictionary<SkinData.Key, SavableSkinData> _skinInfos; // ��Ų �ر� ����

    // ��ųʸ��� ä���ִ� �ڵ� �ʿ�
    // --> ���� ���� �����͸� Ȯ���ϴ� �������� ������ ����
    // �ذ� ��� --> ���� ��ųʸ��� ���ٸ� �ش� �����͸� �׶� ä���ִ´�.
    // --> �� �� �����غ���

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

    // Json ������ ������Ʈ
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

        // �ҷ����� �� ������ �ִٸ� �⺻ ���̺�� �ٲٰ� �ٽ� ����
        try
        {
            _saveData = _parser.JsonToObject<SaveData>(json);
            _saveData.Update(); // ������ �����͸� �ε��� �� ������Ʈ �ʿ�
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            _saveData = _defaultSaveData; // �⺻������ ������Ʈ �Ǿ�����
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
