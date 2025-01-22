using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PhaseData
{
    [JsonProperty] float spawnTime;
    [JsonProperty] SpawnData[] spawnDatas;

    public PhaseData(float spawnTime, SpawnData[] mobSpawnDatas)
    {
        this.spawnTime = spawnTime;
        this.spawnDatas = mobSpawnDatas;
    }

    [JsonIgnore] public float SpawnTime { get => spawnTime; }
    [JsonIgnore] public SpawnData[] SpawnDatas { get => spawnDatas; }
}

[Serializable]
public struct SurvivalStageData : ILevelData
{
    [JsonProperty] PhaseData[] phaseDatas;
    [JsonIgnore] public PhaseData[] PhaseDatas { get { return phaseDatas; } }

    public SurvivalStageData(PhaseData[] phaseDatas)
    {
        this.phaseDatas = phaseDatas;
    }
}