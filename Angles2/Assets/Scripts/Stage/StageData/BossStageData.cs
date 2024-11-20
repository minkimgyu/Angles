using System;
using UnityEngine;

[Serializable]
public struct BossStageData : IStageData
{
    [SerializeField] public SpawnData bossSpawnData;
    [SerializeField] public SpawnData[] mobSpawnDatas;

    public BossStageData(SpawnData bosssSpawnData, SpawnData[] mobSpawnDatas)
    {
        this.bossSpawnData = bosssSpawnData;
        this.mobSpawnDatas = mobSpawnDatas;
    }
}
