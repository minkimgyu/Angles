using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[Serializable]
public struct ChapterLevelData : ILevelData
{
    [JsonProperty] MobStageData[] mobStageData;
    [JsonProperty] BossStageData bossStageData;

    public MobStageData[] MobStageDatas { get { return mobStageData; } }
    public BossStageData BossStageData { get { return bossStageData; } }

    public ChapterLevelData(MobStageData[] mobStageData, BossStageData bossStageData)
    {
        this.mobStageData = mobStageData;
        this.bossStageData = bossStageData;
    }
}