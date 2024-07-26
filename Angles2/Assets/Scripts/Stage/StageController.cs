using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StageController : MonoBehaviour
{
    int _stageCount = 30;
    Queue<BaseStage> _stageQueue;
    BaseStage _currentStage;

    Action OnGameClearRequested;
    Action OnGameOverRequested;

    public void Initialize(Action OnGameClearRequested, Action OnGameOverRequested)
    {
        _stageQueue = new Queue<BaseStage>();

        this.OnGameClearRequested = OnGameClearRequested;
        this.OnGameOverRequested = OnGameOverRequested;
    }

    public void CreateRandomStageQueue(List<BaseStage> stageObjects)
    {
        int randomIndex;
        int beforeIndex = -1;

        BaseStage stage = stageObjects[0];
        _stageQueue.Enqueue(stage);

        for (int i = 0; i < _stageCount; i++)
        {
            randomIndex = UnityEngine.Random.Range(0, stageObjects.Count);
            while (beforeIndex == randomIndex)
            {
                randomIndex = UnityEngine.Random.Range(0, stageObjects.Count);
            }

            BaseStage stage = stageObjects[randomIndex];
            _stageQueue.Enqueue(stage);

            beforeIndex = randomIndex;
        }

        _currentStage = _stageQueue.Dequeue();
        _currentStage.Initialize(OnClearRequested);
    }

    void OnClearRequested()
    {
        if(_stageQueue.Count == 0)
        {
            OnGameClearRequested?.Invoke();
            return;
        }

        BaseStage stage = _stageQueue.Dequeue();
        stage.Initialize(OnClearRequested);

        Vector3 entryPos = stage.ReturnEntryPosition();
        _currentStage.ActivePortal(entryPos);
    }
}
