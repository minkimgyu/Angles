using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelData
{
    public virtual MobStageData[] MobStageDatas { get { return default; } }
    public virtual BossStageData BossStageData { get { return default; } }
    public virtual PhaseData[] PhaseDatas { get { return default; } }
}

[Serializable]
public struct SpawnData
{
    [SerializeField] [JsonProperty] SerializableVector2 spawnPosition; // ������ ��ġ ����
    [SerializeField] [JsonProperty] [JsonConverter(typeof(StringEnumConverter))] BaseLife.Name name; // ������ �� �̸�

    public SpawnData(Vector3 point, BaseLife.Name name)
    {
        spawnPosition = new SerializableVector2(point);
        this.name = name;
    }

    [JsonIgnore] public SerializableVector2 SpawnPosition { get => spawnPosition; }
    [JsonIgnore] public BaseLife.Name Name { get => name; }
}