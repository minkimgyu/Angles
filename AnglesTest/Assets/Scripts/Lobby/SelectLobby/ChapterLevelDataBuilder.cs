using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChapterLevelDataBuilder : MonoBehaviour
{
    [SerializeField] TextAsset[] mobStageDataTxt;
    [SerializeField] TextAsset bossStageDataTxt;

    JsonParser jsonParser;

    public ChapterLevelDataBuilder()
    {
        jsonParser = new JsonParser();
    }

    [ContextMenu("Build Data")]
    public void BuildData()
    {
        FileIO fileIO = new FileIO(jsonParser, ".txt");

        string fileName = "ChapterLevelData";
        string fileLocation = "JsonData";


        MobStageData[] mobStageDatas = new MobStageData[mobStageDataTxt.Length];
        for (int i = 0; i < mobStageDataTxt.Length; i++)
        {
            mobStageDatas[i] = jsonParser.JsonToObject<MobStageData>(mobStageDataTxt[i].text);
        }

        BossStageData bossStageData = jsonParser.JsonToObject<BossStageData>(bossStageDataTxt.text);

        ChapterLevelData chapterLevelData = new ChapterLevelData(mobStageDatas, bossStageData);
        fileIO.SaveData(chapterLevelData, fileLocation, fileName);
    }
}