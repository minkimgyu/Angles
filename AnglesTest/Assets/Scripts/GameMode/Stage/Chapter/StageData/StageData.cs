using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[Serializable]
//public enum Difficulty
//{
//    Easy,
//    Nomal,
//    Hard,
//}

public interface IStageData { }

[Serializable]
public struct SpawnData
{
    [SerializeField] public SerializableVector2 spawnPosition; // 실질적 위치 제공
    [SerializeField] public BaseLife.Name name;

    public SpawnData(Vector3 point, BaseLife.Name name)
    {
        spawnPosition = new SerializableVector2(point);
        this.name = name;
    }
}