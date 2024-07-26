using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    MapGenerator _mapGenerator;
    StageController _stageController;

    // Start is called before the first frame update
    void Start()
    {
        _mapGenerator = GetComponent<MapGenerator>();
        _stageController = GetComponent<StageController>();

        _mapGenerator.Initialize();
        _stageController.Initialize();

        _mapGenerator.CreateMap();
        List<BaseStage> stages = _mapGenerator.ReturnStageObjects();

        _stageController.CreateRandomStageQueue(stages);
    }

    void OnGameOverRequested()
    {

    }

    void OnGameClearRequested()
    {

    }
}
