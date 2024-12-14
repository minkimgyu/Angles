using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator
{
    [SerializeField] Transform _stageParent;
    [SerializeField] Vector3 _offsetFromCenter;

    BaseFactory _stageFactory;

    const int _maxRow = 3;
    const int _offset = 100;

    int xPos = 0;
    int rowCount = 0;

    public MapGenerator(BaseFactory stageFactory)
    {
        _stageFactory = stageFactory;
    }

    public Dictionary<BaseStage.Name, BaseStage> CreateMap(DungeonMode.Chapter chapter)
    {
        Dictionary<BaseStage.Name, BaseStage> stages = new Dictionary<BaseStage.Name, BaseStage>();

        int count = System.Enum.GetValues(typeof(BaseStage.Name)).Length;
        for (int i = 0; i < count; i++)
        {
            BaseStage.Name stageName = (BaseStage.Name)i;
            Vector3 stagePosition = new Vector3(xPos, rowCount * _offset) + _offsetFromCenter;

            BaseStage stage = _stageFactory.Create(chapter, stageName);
            stages.Add(stageName, stage);
            stage.transform.position = stagePosition;

            rowCount++;
            if (rowCount == _maxRow)
            {
                xPos += _offset;
                rowCount = 0;
            }
        }

        return stages;
    }
}
