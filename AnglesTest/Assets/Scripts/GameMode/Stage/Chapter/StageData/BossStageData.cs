using Newtonsoft.Json;
using System;
using UnityEngine;

[Serializable]
public struct BossStageData
{
    [JsonProperty] SpawnData bossSpawnData;
    [JsonProperty] SpawnData[] mobSpawnDatas;

    public BossStageData(SpawnData bosssSpawnData, SpawnData[] mobSpawnDatas)
    {
        this.bossSpawnData = bosssSpawnData;
        this.mobSpawnDatas = mobSpawnDatas;
    }

    [JsonIgnore] public SpawnData[] MobSpawnDatas { get => mobSpawnDatas; }
    [JsonIgnore] public SpawnData BossSpawnData { get => bossSpawnData; }
}
