using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] Transform _stageParent;
    [SerializeField] Vector3 _offsetFromCenter;

    Dictionary<BaseStage.Type, List<BaseStage>> _stagePrefabs;
    public Dictionary<BaseStage.Type, List<BaseStage>> StageObjects { get; private set; }

    const int _maxRow = 3;
    const int _offset = 100;

    int xPos = 0;
    int rowCount = 0;

    public void Initialize(Dictionary<BaseStage.Type, List<BaseStage>> stagePrefabs)
    {
        _stagePrefabs = stagePrefabs;
        StageObjects = new Dictionary<BaseStage.Type, List<BaseStage>>();
    }

    public void CreateMap()
    {
        foreach (BaseStage.Type stageType in Enum.GetValues(typeof(BaseStage.Type)))
        {
            CreateStage(stageType, _stagePrefabs[stageType]);
        }
    }

    void CreateStage(BaseStage.Type type, List<BaseStage> stagePrefabs)
    {
        List<BaseStage> stages = new List<BaseStage>();

        for (int i = 0; i < stagePrefabs.Count; i++)
        {
            Vector3 stagePosition = new Vector3(xPos, rowCount * _offset) + _offsetFromCenter;
            BaseStage stage = Instantiate(stagePrefabs[i], _stageParent);
            stage.transform.position = stagePosition;

            stages.Add(stage);

            rowCount++;
            if (rowCount == _maxRow)
            {
                xPos += _offset;
                rowCount = 0;
            }
        }

        StageObjects.Add(type, stages);
    }
}
