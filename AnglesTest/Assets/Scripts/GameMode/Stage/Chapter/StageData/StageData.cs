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
    [SerializeField] public SerializableVector2 spawnPosition; // ������ ��ġ ����
    [SerializeField] public BaseLife.Name name;

    public SpawnData(Vector3 point, BaseLife.Name name)
    {
        spawnPosition = new SerializableVector2(point);
        this.name = name;
    }
}