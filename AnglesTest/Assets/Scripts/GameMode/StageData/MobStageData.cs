using Newtonsoft.Json;
using System;
using UnityEngine;

[Serializable]
public struct MobStageData
{
    [JsonProperty] SpawnData[] easySpawnDatas;
    [JsonProperty] SpawnData[] normalSpawnDatas;
    [JsonProperty] SpawnData[] hardSpawnDatas;

    public MobStageData(
        SpawnData[] easySpawnDatas,
        SpawnData[] normalSpawnDatas,
        SpawnData[] hardSpawnDatas)
    {
        this.easySpawnDatas = easySpawnDatas;
        this.normalSpawnDatas = normalSpawnDatas;
        this.hardSpawnDatas = hardSpawnDatas;
    }

    public SpawnData[] GetSpawnData(MobStage.Difficulty difficulty)
    {
        switch (difficulty)
        {
            case MobStage.Difficulty.Easy:
                return easySpawnDatas;
            case MobStage.Difficulty.Nomal:
                return normalSpawnDatas;
            case MobStage.Difficulty.Hard:
                return hardSpawnDatas;
            default:
                return default;
        }
    }
}