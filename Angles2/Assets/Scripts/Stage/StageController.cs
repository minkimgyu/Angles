using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StageController : MonoBehaviour
{
    public struct Datas
    {
        public Datas(
            int stageCount,
            int bonusStageGap,
            IFactory factory)
        {
            _stageCount = stageCount;
            _bonusStageGap = bonusStageGap;
            _factory = factory;
        }

        int _stageCount;
        public int StageCount { get { return _stageCount; } }

        int _bonusStageGap;
        public int BonusStageGap { get { return _bonusStageGap; } }

        IFactory _factory;
        public IFactory Factory { get { return _factory; } }
    }

    public struct Events
    {
        public Events(
            DungeonSystem.CommandCollection commandCollection,
            DungeonSystem.ObserverEventCollection eventCollection
        )
        {
            _commandCollection = commandCollection;
            _eventCollection = eventCollection;
        }

        DungeonSystem.CommandCollection _commandCollection;
        public DungeonSystem.CommandCollection CommandCollection { get { return _commandCollection; } }

        DungeonSystem.ObserverEventCollection _eventCollection;
        public DungeonSystem.ObserverEventCollection EventCollection { get { return _eventCollection; } }
    }

    [SerializeField] int _maxStageCount = 2;
    [SerializeField] int _stageCount = 1;

    Queue<BaseStage> _stageQueue;

    BaseStage _currentStage;
    BaseStage _nextStage;

    Datas _datas;
    Events _events;

    int _bonusStageGap = 5;

    [SerializeField] BaseViewer _stageCountViewer;
    [SerializeField] BaseViewer _stageResultViewer;

    public void Initialize(Datas data, Events events)
    {
        _datas = data;
        _events = events;

        _stageResultViewer.TurnOnViewer(false);
        _stageQueue = new Queue<BaseStage>();
        _currentStage = null;
        _nextStage = null;
    }

    // 스테이지 출입, 클리어, 탈출 이벤트 제공

    // 탈출 --> 출입 시 플레이어 데이터 제공해줌
    // 스테이지 간의 데이터 전달 기능 추가

    void OnStageClearRequested()
    {
        _stageResultViewer.UpdateViewer("Stage Clear");

        if (_stageQueue.Count == 0)
        {
            _events.EventCollection.OnGameClearRequested?.Invoke();
            return;
        }

        _nextStage = _stageQueue.Dequeue();
        Vector3 entryPos = _nextStage.ReturnEntryPosition();
        _currentStage.ActivePortal(entryPos);
    }

    void OnMoveToNextStageRequested()
    {
        _stageCount++;
        _stageResultViewer.TurnOnViewer(false);
        _stageCountViewer.UpdateViewer(_stageCount);
        _currentStage.Exit();

        _nextStage.Target = _currentStage.Target;
        _currentStage = _nextStage;
        _nextStage = null;

        _currentStage.Spawn(_maxStageCount, _maxStageCount - _stageQueue.Count, _datas.Factory);
    }

    BaseStage ReturnRandomStage(BaseStage.Type type, Dictionary<BaseStage.Type, List<BaseStage>> stageObjects)
    {
        int startStageCount = stageObjects[type].Count;
        return stageObjects[type][UnityEngine.Random.Range(0, startStageCount)];
    }

    public void CreateRandomStage(Dictionary<BaseStage.Type, List<BaseStage>> stageObjects)
    {
        BaseStage.Events events = new BaseStage.Events
        (
            _events.CommandCollection,
            _events.EventCollection,

            OnStageClearRequested,
            OnMoveToNextStageRequested
        );

        foreach (var item in stageObjects)
        {
            for (int i = 0; i < item.Value.Count; i++)
            {
                item.Value[i].Initialize(events);
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
        _currentStage.Spawn(_maxStageCount, _maxStageCount - _stageQueue.Count, _datas.Factory);
        _stageCountViewer.UpdateViewer(_stageCount);
    }
}
