using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
public interface ISaveable
{
    void Save();
    void ClearSave();

    void Load();

    void ChangeBGMMute(bool nowMute);
    void ChangeSFXMute(bool nowMute);

    void ChangeSkin(SkinData.Key name);
    void UnlockSkin(SkinData.Key name);

    void ChangeStat(StatData.Key name, int level);

    void ChangeLevelProgress(GameMode.Type type, GameMode.Level level, int completeLevel);
    void ChangeLevelDuration(GameMode.Type type, GameMode.Level level, int completeDuration);
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

    public void ChangeLevelProgress(GameMode.Type type, GameMode.Level level, int completeLevel) { }
    public void ChangeLevelDuration(GameMode.Type type, GameMode.Level level, int completeDuration) { }
    public void ChangeCurrentLevel(GameMode.Type type, GameMode.Level level) { }
}

public struct SaveData
{
    public bool _muteBGM;
    public bool _muteSFX;

    public int _gold; // ���
    public Dictionary<GameMode.Type, GameMode.Level> _selectedLevel; // ���õ� �������� ����

    public SkinData.Key _skin;
    public Dictionary<GameMode.Type, Dictionary<GameMode.Level, ISavableLevelInfo>> _levelTypeInfos; // ���� ����

    public Dictionary<StatData.Key, SavableStatData> _statLevelInfos; // ���� ���� ����
    public Dictionary<SkinData.Key, SavableSkinData> _skinLockInfos; // ��Ų �ر� ����

    // ��ųʸ��� ä���ִ� �ڵ� �ʿ�
    // --> ���� ���� �����͸� Ȯ���ϴ� �������� ������ ����
    // �ذ� ��� --> ���� ��ųʸ��� ���ٸ� �ش� �����͸� �׶� ä���ִ´�.
    // --> �� �� �����غ���

    public SaveData(int gold, bool muteBGM = false, bool muteSFX = false)
    {
        _muteBGM = muteBGM;
        _muteSFX = muteSFX;
        _gold = gold;

        _selectedLevel = new Dictionary<GameMode.Type, GameMode.Level>();
        _selectedLevel[GameMode.Type.Chapter] = GameMode.Level.TriconChapter;
        _selectedLevel[GameMode.Type.Survival] = GameMode.Level.CubeSurvival;

        _skin = SkinData.Key.Normal;

        _levelTypeInfos = new Dictionary<GameMode.Type, Dictionary<GameMode.Level, ISavableLevelInfo>>()
        {
            {
                GameMode.Type.Chapter,
                new Dictionary<GameMode.Level, ISavableLevelInfo>()
                {
                    { GameMode.Level.TriconChapter, new SavableChapterInfo(0, true) },
                    { GameMode.Level.RhombusChapter, new SavableChapterInfo(0, false) },
                    { GameMode.Level.PentagonicChapter, new SavableChapterInfo(0, false) },
                }
            },
            {
                GameMode.Type.Survival,
                new Dictionary<GameMode.Level, ISavableLevelInfo>()
                {
                    { GameMode.Level.CubeSurvival, new SavableSurvivalInfo(0, true) },
                    { GameMode.Level.PyramidSurvival, new SavableSurvivalInfo(0, false) },
                    { GameMode.Level.PrismSurvival, new SavableSurvivalInfo(0, false) },
                }
            }
        };

        _statLevelInfos = new Dictionary<StatData.Key, SavableStatData>();
        foreach (StatData.Key i in Enum.GetValues(typeof(StatData.Key)))
        {
            _statLevelInfos.Add(i, new SavableStatData(0));
        }

        _skinLockInfos = new Dictionary<SkinData.Key, SavableSkinData>();
        foreach (SkinData.Key i in Enum.GetValues(typeof(SkinData.Key)))
        {
            if (i == 0) _skinLockInfos.Add(i, new SavableSkinData(true));
            else _skinLockInfos.Add(i, new SavableSkinData(false));
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

    public void ChangeSkin(SkinData.Key name)
    {
        _saveData._skin = name;
        Save();
    }

    public void UnlockSkin(SkinData.Key name)
    {
        SavableSkinData savableSkinData = _saveData._skinLockInfos[name];
        savableSkinData._nowUnlock = true;

        _saveData._skinLockInfos[name] = savableSkinData;
        Save();
    }

    public void ChangeStat(StatData.Key name, int level)
    {
        SavableStatData savableStatData = _saveData._statLevelInfos[name];
        savableStatData._currentLevel = level;

        _saveData._statLevelInfos[name] = savableStatData;
        Save();
    }

    public void ChangeLevelProgress(GameMode.Type type, GameMode.Level level, int completeLevel)
    {
        ISavableLevelInfo savableChapterInfo = _saveData._levelTypeInfos[type][level];
        savableChapterInfo.CompleteLevel = completeLevel;

        _saveData._levelTypeInfos[type][level] = savableChapterInfo;
        Save();
    }

    public void ChangeLevelDuration(GameMode.Type type, GameMode.Level level, int completeDuration)
    {
        ISavableLevelInfo savableChapterInfo = _saveData._levelTypeInfos[type][level];
        savableChapterInfo.CompleteDuration = completeDuration;

        _saveData._levelTypeInfos[type][level] = savableChapterInfo;
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
        ISavableLevelInfo savableChapterInfo = _saveData._levelTypeInfos[type][level];
        savableChapterInfo.NowUnlock = true;

        _saveData._levelTypeInfos[type][level] = savableChapterInfo;
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
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            _saveData = _defaultSaveData;
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
