using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public interface ISaveable
{
    void Save();
    void Load();

    void EquipSkin(SkinData.Key name);
    void UnlockSkin(SkinData.Key name);

    void ChangeStat(StatData.Key name, int level);
    void ChangeStageProgress(DungeonMode.Chapter chapter, int level);
    void ChangeCurrentStage(DungeonMode.Chapter chapter);

    /// <summary>
    /// ������ �߰��Ѵ�.
    /// </summary>
    void AddCoinCount(int cointCount);

    /// <summary>
    /// ���� ������ �������ش�.
    /// </summary>
    void ChangeCoinCount(int cointCount);

    void UnlockChapter(DungeonMode.Chapter chapter);
    SaveData GetSaveData() { return default; }
}

public class NULLSaveManager : ISaveable
{
    public void Load() { }
    public void Save() { }
    public SaveData GetSaveData() { return default; }

    public void ChangeStageProgress(DungeonMode.Chapter chapter, int level) { }
    public void ChangeCurrentStage(DungeonMode.Chapter chapter) { }

    public void AddCoinCount(int cointCount) { }
    public void ChangeCoinCount(int cointCount) { }

    public void UnlockChapter(DungeonMode.Chapter chapter) { }
    public void ChangeStat(StatData.Key name, int level) { }

    public void EquipSkin(SkinData.Key name) { }
    public void UnlockSkin(SkinData.Key name) { }
}

public struct SaveData
{
    public int _gold; // ���
    public DungeonMode.Chapter _chapter; // ���� ������ é��
    public Dictionary<DungeonMode.Chapter, ChapterInfo> _chapterInfos; // é�� ����
    public Dictionary<StatData.Key, int> _statLevelInfos; // ���� ���� ����

    public SkinData.Key _skin;
    public Dictionary<SkinData.Key, bool> _skinLockInfos; // ��Ų �ر� ����

    // ��ųʸ��� ä���ִ� �ڵ� �ʿ�
    // --> ���� ���� �����͸� Ȯ���ϴ� �������� ������ ����

    public SaveData(
        int gold,
        DungeonMode.Chapter chapter,
        Dictionary<DungeonMode.Chapter, ChapterInfo> chapterInfos,
        Dictionary<StatData.Key, int> statLevelInfos,

        SkinData.Key skin,
        Dictionary<SkinData.Key, bool> skinLockInfos)
    {
        _gold = gold;
        _chapter = chapter;
        _chapterInfos = chapterInfos;
        _statLevelInfos = statLevelInfos;

        _skin = skin;
        _skinLockInfos = skinLockInfos;
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

    public void EquipSkin(SkinData.Key name)
    {
        _saveData._skin = name;
        Save();
    }

    public void UnlockSkin(SkinData.Key name)
    {
        _saveData._skinLockInfos[name] = false;
        Save();
    }

    public void ChangeStat(StatData.Key name, int level)
    {
        Dictionary<StatData.Key, int> statLevelInfos = _saveData._statLevelInfos;
        statLevelInfos[name] = level;
        Save();
    }

    public void ChangeStageProgress(DungeonMode.Chapter chapter, int level)
    {
        Dictionary<DungeonMode.Chapter, ChapterInfo> chapterInfos = _saveData._chapterInfos;
        ChapterInfo chapterInfo = chapterInfos[chapter];

        chapterInfo._completeLevel = level;
        chapterInfos[chapter] = chapterInfo;
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

    public void UnlockChapter(DungeonMode.Chapter chapter) 
    {
        Dictionary<DungeonMode.Chapter, ChapterInfo> chapterInfos = _saveData._chapterInfos;
        ChapterInfo chapterInfo = chapterInfos[chapter];

        chapterInfo._nowLock = false;
        chapterInfos[chapter] = chapterInfo;
        Save();
    }

    public void ChangeCurrentStage(DungeonMode.Chapter chapter)
    {
        _saveData._chapter = chapter;
        Save();
    }

    public SaveData GetSaveData()
    {
        return _saveData;
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
        _saveData = _parser.JsonToObject<SaveData>(json);
    }

    public void Save()
    {
        string json = _parser.ObjectToJson(_saveData);
        File.WriteAllText(_filePath, json);
    }
}
