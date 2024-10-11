using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Unity.VisualScripting.Antlr3.Runtime;

public interface ISaveable
{
    void Save();
    void Load();

    void ChangeStageProgress(DungeonChapter chapter, int level);

    void ChangeCurrentStage(DungeonChapter chapter);
    void AddCoinCount(int cointCount);
    void UnlockChapter(DungeonChapter chapter);
    SaveData ReturnSaveData() { return default; }
}

public class NULLSaveManager : ISaveable
{
    public void Load() { }
    public void Save() { }
    public SaveData ReturnSaveData() { return default; }

    public void ChangeStageProgress(DungeonChapter chapter, int level) { }
    public void ChangeCurrentStage(DungeonChapter chapter) { }
    public void AddCoinCount(int cointCount) { }
    public void UnlockChapter(DungeonChapter chapter) { }
}

public struct SaveData
{
    public int _gold; // ∞ÒµÂ
    public DungeonChapter _chapter; // «ˆ¿Á º±≈√«— √©≈Õ
    public Dictionary<DungeonChapter, ChapterInfo> _chapterInfos; // √©≈Õ ¡§∫∏

    public SaveData(
        int gold,
        DungeonChapter chapter,
        Dictionary<DungeonChapter, ChapterInfo> chapterInfos)
    {
        _gold = gold;
        _chapter = chapter;
        _chapterInfos = chapterInfos;
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

    public void ChangeStageProgress(DungeonChapter chapter, int level)
    {
        Dictionary<DungeonChapter, ChapterInfo> chapterInfos = _saveData._chapterInfos;
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

    public void UnlockChapter(DungeonChapter chapter) 
    {
        Dictionary<DungeonChapter, ChapterInfo> chapterInfos = _saveData._chapterInfos;
        ChapterInfo chapterInfo = chapterInfos[chapter];

        chapterInfo._nowLock = false;
        chapterInfos[chapter] = chapterInfo;
        Save();
    }

    public void ChangeCurrentStage(DungeonChapter chapter)
    {
        _saveData._chapter = chapter;
        Save();
    }

    public SaveData ReturnSaveData()
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
