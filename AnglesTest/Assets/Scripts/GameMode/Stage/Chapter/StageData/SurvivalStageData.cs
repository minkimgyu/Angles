using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PhaseData
{
    [SerializeField] public float spawnTime;
    [SerializeField] public SpawnData[] spawnDatas;

    public PhaseData(float spawnTime, SpawnData[] mobSpawnDatas)
    {
        this.spawnTime = spawnTime;
        this.spawnDatas = mobSpawnDatas;
    }
}

[Serializable]
public class SurvivalStageData
{
    [SerializeField] public PhaseData[] phaseDatas;

    public SurvivalStageData(PhaseData[] phaseDatas)
    {
        this.phaseDatas = phaseDatas;
    }
}