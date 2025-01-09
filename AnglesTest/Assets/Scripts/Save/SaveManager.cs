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
    /// ������ �߰��Ѵ�.
    /// </summary>
    virtual void AddCoinCount(int cointCount) { }

    /// <summary>
    /// ���� ������ �������ش�.
    /// </summary>
    virtual void ChangeCoinCount(int cointCount) { }

    virtual void UnlockLevel(GameMode.Level level) { }

    /// <summary>
    /// ���̺� ������ ��������
    /// </summary>
    /// <returns></returns>
    virtual SaveData GetSaveData() { return default; }

    /// <summary>
    /// ���̺� Json ������ ��������
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

    public int _gold; // ���

    [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
    public Dictionary<GameMode.Type, GameMode.Level> _selectedLevel; // Ÿ�Ժ� ���õ� �������� ����

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

    // Json ������ ������Ʈ
    public void Update()
    {
        // ���� Chapter Ű�� ��� ������ ���ٸ� ����
        if (_selectedLevel.ContainsKey(GameMode.Type.Chapter) == false)
        {
            _selectedLevel[GameMode.Type.Chapter] = GameMode.Level.TriconChapter;
        }

        // ���� Survival Ű�� ��� ������ ���ٸ� ����
        if (_selectedLevel.ContainsKey(GameMode.Type.Survival) == false)
        {
            _selectedLevel[GameMode.Type.Survival] = GameMode.Level.PyramidSurvival;
        }

        // ���� �߰��� �����͸� �⺻ �����ͷ� �߰����ش�.
        List<GameMode.Level> chapterLevels = GameMode.GetLevels(GameMode.Type.Chapter);
        for (int i = 0; i < chapterLevels.Count; i++)
        {
            if (_levelInfos.ContainsKey(chapterLevels[i]) == true) continue;

            _levelInfos[chapterLevels[i]] = new SavableChapterInfo(0, false);
        }

        // ���� �߰��� �����͸� �⺻ �����ͷ� �߰����ش�.
        List<GameMode.Level> survivalLevels = GameMode.GetLevels(GameMode.Type.Survival);
        for (int i = 0; i < survivalLevels.Count; i++)
        {
            if (_levelInfos.ContainsKey(survivalLevels[i]) == true) continue;

            _levelInfos[survivalLevels[i]] = new SavableSurvivalInfo(0, false);
        }

        // ���� �߰��� �����͸� �⺻ �����ͷ� �߰����ش�.
        foreach (StatData.Key key in Enum.GetValues(typeof(StatData.Key)))
        {
            if(_statInfos.ContainsKey(key) == false) _statInfos.Add(key, new SavableStatData(0));
        }

        // ���� �߰��� �����͸� �⺻ �����ͷ� �߰����ش�.
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
        // ������ �������� �ʴ´ٸ�
        if (!HaveSaveFile())
        {
            _saveData = _defaultSaveData;
            Save(); // ���̺� ������ ������ְ� �����Ѵ�.
        }

        // ����� ������ �ҷ��� �����Ѵ�.
        string json = File.ReadAllText(_filePath);
        return json;
    }


    /// <summary>
    /// GPGS�� ������ ����
    /// ���� �������� ���� �����Ͱ� ���峭 ��� ���� �����͸� �������� �ʰ�
    /// �״�� ���
    /// </summary>

    public bool VerifyJson(string json)
    {
        // �ҷ����� �� ������ �ִٸ� �� �̻� �������� ����
        try
        {
            SaveData newSaveData = _parser.JsonToObject<SaveData>(json);
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            return false; // ��ȿ���� ����
        }

        return true; // ��ȿ��
    }


    public bool VerifyJson(string json, out SaveData saveData)
    {
        // �ҷ����� �� ������ �ִٸ� �� �̻� �������� ����
        try
        {
            saveData = _parser.JsonToObject<SaveData>(json);
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            saveData = default;
            return false; // ��ȿ���� ����
        }

        return true; // ��ȿ��
    }

    public bool HaveSaveFile()
    {
        return File.Exists(_filePath);
    }

    public void Load()
    {
        // ������ �������� �ʴ´ٸ�
        if (!HaveSaveFile())
        {
            _saveData = _defaultSaveData; // �⺻ ���̺�� ��ü���ش�.
            Save();
            return;
        }

        string json = File.ReadAllText(_filePath);

        SaveData newSaveData;
        bool nowValidate = VerifyJson(json, out newSaveData);

        if(nowValidate == true)
        {
            _saveData = newSaveData; // ��ȿ�ϴٸ� �ش� �����͸� ����Ѵ�.
            _saveData.Update(); // ���� �����͸� ������Ʈ ���ش�.
        }
        else
        {
            _saveData = _defaultSaveData; // �⺻ �����ͷ� ��ü�Ѵ�.
            Save();
        }
    }

    /// <summary>
    /// GPGS�� ���̺�
    /// </summary>
    public void Save(string saveJson)
    {
        bool nowValidate = VerifyJson(saveJson);
        if (nowValidate == false) return; // ��ȿ���� �ʴٸ� �������� ����

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
