using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DungeonStageController : BaseStageController
{
    int _maxStageCount;
    int _stageCount;

    Queue<BaseStage> _stageQueue;

    BaseStage _currentStage;
    BaseStage _nextStage;

    const int _bonusStageGap = 5;

    // �� ���� model���� ��������
    [SerializeField] StageUIController _stageUIController;
    CoreSystem _coreSystem;

    public int ReturnCurrentStageCount() { return _stageCount; }

    public void Initialize(int maxStageCount, CoreSystem coreSystem)
    {
        _stageCount = 1;
        _maxStageCount = maxStageCount;
        _coreSystem = coreSystem;

        _stageUIController.Initialize();
        _stageUIController.ShowStageResult(false);
        _stageQueue = new Queue<BaseStage>();
        _currentStage = null;
        _nextStage = null;
    }

    // �������� ����, Ŭ����, Ż�� �̺�Ʈ ����

    // Ż�� --> ���� �� �÷��̾� ������ ��������
    // �������� ���� ������ ���� ��� �߰�

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

        if (_stageCount == _maxStageCount)  // ������ ���������� ���
        {
            _stageUIController.ShowStageCountViewer(false);
            _stageUIController.ShowBossHpViewer(true);
            _currentStage.AddBossHPEvent(_stageUIController.ChangeBossHpRatio);
        }
        _currentStage.Spawn(_maxStageCount, _maxStageCount - _stageQueue.Count);
    }

    BaseStage ReturnRandomStage(BaseStage.Type type, Dictionary<BaseStage.Type, List<BaseStage>> stageObjects)
    {
        int startStageCount = stageObjects[type].Count;
        return stageObjects[type][UnityEngine.Random.Range(0, startStageCount)];
    }


    //���� 0
    //�Ϲ� 1
    //�Ϲ� 2
    //�Ϲ� 3
    //���ʽ� 4

    //�Ϲ� 5
    //�Ϲ� 6
    //�Ϲ� 7
    //�Ϲ� 8
    //���ʽ� 9

    //�Ϲ� 10
    //�Ϲ� 11
    //�Ϲ� 12
    //�Ϲ� 13
    //���ʽ� 14

    //�Ϲ� 15
    //�Ϲ� 16
    //�Ϲ� 17
    //�Ϲ� 18
    //���� 19


    public void CreateRandomStage(Dictionary<BaseStage.Type, List<BaseStage>> stageObjects)
    {
        foreach (var item in stageObjects)
        {
            for (int i = 0; i < item.Value.Count; i++)
            {
                item.Value[i].Initialize(this, _coreSystem);
            }
        }

        BaseStage storedBattleStage = null;
        BaseStage startStage = ReturnRandomStage(BaseStage.Type.StartStage, stageObjects);
        _stageQueue.Enqueue(startStage);

        for (int i = 1; i < _maxStageCount; i++)
        {
            if(i == _maxStageCount - 1)
            {
                BaseStage bossStage = ReturnRandomStage(BaseStage.Type.BossStage, stageObjects);
                _stageQueue.Enqueue(bossStage);
            }
            else if((i + 1) % _bonusStageGap == 0)
            {
                BaseStage bonusStage = ReturnRandomStage(BaseStage.Type.BonusStage, stageObjects);
                _stageQueue.Enqueue(bonusStage);
            }
            else
            {
                BaseStage battleStage = ReturnRandomStage(BaseStage.Type.MobStage, stageObjects);

                if(storedBattleStage == null)
                {
                    _stageQueue.Enqueue(battleStage);
                    storedBattleStage = battleStage;
                }
                else
                {
                    while (storedBattleStage == battleStage) battleStage = ReturnRandomStage(BaseStage.Type.MobStage, stageObjects);

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
