using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChapterStageController : BaseStageController
{
    int _maxStageCount;
    int _stageCount;

    Queue<BaseStage> _stageQueue;

    BaseStage _currentStage;
    BaseStage _nextStage;

    const int _bonusStageGap = 5;

    // 이 둘은 model에서 관리하자
    [SerializeField] StageUIController _stageUIController;
    AddressableHandler _addressableHandler;
    InGameFactory _inGameFactory;

    public int ReturnCurrentStageCount() { return _stageCount; }

    public void Initialize(int maxStageCount, AddressableHandler addressableHandler, InGameFactory inGameFactory)
    {
        _stageCount = 1;
        _maxStageCount = maxStageCount;

        _addressableHandler = addressableHandler;
        _inGameFactory = inGameFactory;

        _stageUIController.Initialize();
        _stageUIController.ShowStageResult(false);
        _stageQueue = new Queue<BaseStage>();

        _mapGenerator = new MapGenerator(inGameFactory.GetFactory(InGameFactory.Type.Stage));

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
            EventBusManager.Instance.MainEventBus.Publish(MainEventBus.State.GameClear);
            return;
        }

        _nextStage = _stageQueue.Dequeue();
        Vector3 entryPos = _nextStage.ReturnEntryPosition();
        _currentStage.ActivePortal(entryPos);
    }

    public override void OnMoveToNextStageRequested()
    {
        _stageCount++;
        _stageUIController.AddStageCount(1);
        _stageUIController.ShowStageResult(false);
        _currentStage.Exit();

        _currentStage = _nextStage;
        _nextStage = null;

        if (_stageCount == _maxStageCount)  // 마지막 스테이지의 경우
        {
            _stageUIController.ShowStageCountViewer(false);
            _stageUIController.ShowBossHpViewer(true);
            _currentStage.AddBossHPEvent(_stageUIController.ChangeBossHpRatio);
        }
        _currentStage.Spawn(_maxStageCount, _maxStageCount - _stageQueue.Count);
    }

    BaseStage ReturnRandomMobStage(Dictionary<BaseStage.Name, BaseStage> stageObjects)
    {
        int randomRange = UnityEngine.Random.Range(3, 8); // 3, 4, 5, 6, 7
        return stageObjects[(BaseStage.Name)randomRange];
    }


    //시작 0
    //일반 1
    //일반 2
    //일반 3
    //보너스 4

    //일반 5
    //일반 6
    //일반 7
    //일반 8
    //보너스 9

    //일반 10
    //일반 11
    //일반 12
    //일반 13
    //보너스 14

    //일반 15
    //일반 16
    //일반 17
    //일반 18
    //보스 19

    MapGenerator _mapGenerator;

    public void CreateRandomStage(GameMode.Level level)
    {
        Dictionary<BaseStage.Name, BaseStage> stageObjects = _mapGenerator.CreateMap(level);

        foreach (var item in stageObjects)
        {
            item.Value.Initialize(this, _addressableHandler, _inGameFactory);
        }

        BaseStage storedBattleStage = null;
        BaseStage startStage = stageObjects[BaseStage.Name.StartStage];
        _stageQueue.Enqueue(startStage);

        for (int i = 1; i < _maxStageCount; i++)
        {
            if(i == _maxStageCount - 1)
            {
                BaseStage bossStage = stageObjects[BaseStage.Name.BossStage];
                _stageQueue.Enqueue(bossStage);
            }
            else if((i + 1) % _bonusStageGap == 0)
            {
                BaseStage bonusStage = stageObjects[BaseStage.Name.BonusStage];
                _stageQueue.Enqueue(bonusStage);
            }
            else
            {
                BaseStage battleStage = ReturnRandomMobStage(stageObjects);

                if(storedBattleStage == null)
                {
                    _stageQueue.Enqueue(battleStage);
                    storedBattleStage = battleStage;
                }
                else
                {
                    while (storedBattleStage == battleStage) battleStage = ReturnRandomMobStage(stageObjects);

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
