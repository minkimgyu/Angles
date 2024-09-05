using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DungeonStageController : BaseStageController
{
    [SerializeField] int _maxStageCount = 2;
    [SerializeField] int _stageCount = 1;

    Queue<BaseStage> _stageQueue;

    BaseStage _currentStage;
    BaseStage _nextStage;

    int _bonusStageGap = 5;

    // 이 둘은 model에서 관리하자
    [SerializeField] StageUIController _stageUIController;

    BaseGameMode _gameMode;
    FactoryCollection _factoryCollection;

    public void Initialize(int stageCount, int bonusStageGap, FactoryCollection factoryCollection)
    {
        _stageCount = stageCount;
        _bonusStageGap = bonusStageGap;
        _factoryCollection = factoryCollection;

        _stageUIController.Initialize();
        _stageUIController.ShowStageResult(false);
        _stageQueue = new Queue<BaseStage>();
        _currentStage = null;
        _nextStage = null;
    }

    // 스테이지 출입, 클리어, 탈출 이벤트 제공

    // 탈출 --> 출입 시 플레이어 데이터 제공해줌
    // 스테이지 간의 데이터 전달 기능 추가

    public override void OnStageClearRequested()
    {
        _stageUIController.ShowStageResult(true);
        _stageUIController.ChangeStageResultInfo("Stage Clear");

        if (_stageQueue.Count == 0)
        {
            MainEventBus.Publish(MainEventBus.State.GameClear);
            return;
        }

        _nextStage = _stageQueue.Dequeue();
        Vector3 entryPos = _nextStage.ReturnEntryPosition();
        _currentStage.ActivePortal(entryPos);
    }

    public override void OnMoveToNextStageRequested()
    {
        _stageCount++;
        _stageUIController.ShowStageResult(false);
        _stageUIController.AddStageCount(1);
        _currentStage.Exit();

        _nextStage.Target = _currentStage.Target;
        _currentStage = _nextStage;
        _nextStage = null;

        _currentStage.Spawn(_maxStageCount, _maxStageCount - _stageQueue.Count);
    }

    BaseStage ReturnRandomStage(BaseStage.Type type, Dictionary<BaseStage.Type, List<BaseStage>> stageObjects)
    {
        int startStageCount = stageObjects[type].Count;
        return stageObjects[type][UnityEngine.Random.Range(0, startStageCount)];
    }

    public void CreateRandomStage(Dictionary<BaseStage.Type, List<BaseStage>> stageObjects)
    {
        foreach (var item in stageObjects)
        {
            for (int i = 0; i < item.Value.Count; i++)
            {
                item.Value[i].Initialize(this, _factoryCollection);
            }
        }

        BaseStage storedBattleStage = null;
        BaseStage startStage = ReturnRandomStage(BaseStage.Type.Start, stageObjects);
        _stageQueue.Enqueue(startStage);

        for (int i = 1; i < _maxStageCount; i++)
        {
            if(i % _bonusStageGap == 0)
            {
                BaseStage bonusStage = ReturnRandomStage(BaseStage.Type.Bonus, stageObjects);
                _stageQueue.Enqueue(bonusStage);
            }
            else
            {
                BaseStage battleStage = ReturnRandomStage(BaseStage.Type.Battle, stageObjects);

                if(storedBattleStage == null)
                {
                    _stageQueue.Enqueue(battleStage);
                    storedBattleStage = battleStage;
                }
                else
                {
                    while (storedBattleStage == battleStage) battleStage = ReturnRandomStage(BaseStage.Type.Battle, stageObjects);

                    _stageQueue.Enqueue(battleStage);
                    storedBattleStage = battleStage;
                }
            }
        }

        _currentStage = _stageQueue.Dequeue();
        _currentStage.Spawn(_maxStageCount, _maxStageCount - _stageQueue.Count);
        _stageUIController.AddStageCount(1);
    }
}
