using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TutorialStageData : ILevelData
{
    [JsonProperty] SpawnData[] spawnDatas;
    [JsonIgnore] public SpawnData[] SpawnDatas { get { return spawnDatas; } }

    public TutorialStageData(SpawnData[] spawnDatas)
    {
        this.spawnDatas = spawnDatas;
    }
}
