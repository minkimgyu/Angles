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
    [SerializeField] ChapterLevelUIController _levelUIController;
    AddressableHandler _addressableHandler;
    InGameFactory _inGameFactory;

    PlayerSpawner _playerSpawner;

    public int ReturnCurrentStageCount() { return _stageCount; }

    public void Initialize(int maxStageCount, AddressableHandler addressableHandler, InGameFactory inGameFactory)
    {
        _stageCount = 1;
        _maxStageCount = maxStageCount;

        _addressableHandler = addressableHandler;
        _inGameFactory = inGameFactory;

        _levelUIController.Initialize();
        _levelUIController.ShowStageResult(false);
        _stageQueue = new Queue<BaseStage>();

        _currentStage = null;
        _nextStage = null;

        InputController inputController = FindObjectOfType<InputController>();
        _playerSpawner = new PlayerSpawner(
            inGameFactory,
            inputController,
            addressableHandler.SkinIconAsset,
            addressableHandler.Database.SkinDatas,
            addressableHandler.Database.SkinModifiers,
            addressableHandler.Database.StatDatas,
            addressableHandler.Database.StatModifiers
        );
    }

    // 스테이지 출입, 클리어, 탈출 이벤트 제공

    // 탈출 --> 출입 시 플레이어 데이터 제공해줌
    // 스테이지 간의 데이터 전달 기능 추가

    public override void OnStageClearRequested()
    {
        _levelUIController.ShowStageResult(true);

        string clearTxt = "";
        if(_stageCount == _maxStageCount) clearTxt = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.BossClear);
        else clearTxt = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.StageClear);

        _levelUIController.ChangeStageResultInfo(clearTxt);

        if(_stageQueue.Count == 0)
        {
            _currentStage.ActivePortal();
        }
        else
        {
            _nextStage = _stageQueue.Dequeue();
            Vector3 entryPos = _nextStage.ReturnEntryPosition();
            _currentStage.ActivePortal(entryPos);
        }
    }

    public override void OnMoveToNextStageRequested()
    {
        if (_stageCount == _maxStageCount) // 게임 클리어 이벤트 적용
        {
            EventBusManager.Instance.MainEventBus.Publish(MainEventBus.State.GameClear);
            return;
        }

        _stageCount++;
        _levelUIController.AddStageCount(1);
        _levelUIController.ShowStageResult(false);
        _currentStage.Exit();

        _currentStage = _nextStage;
        _nextStage = null;

        if (_stageCount == _maxStageCount)  // 마지막 스테이지의 경우
        {
            _levelUIController.ShowStageCountViewer(false);
            _levelUIController.ShowBossHpViewer(true);
            _currentStage.AddBossHPEvent(_levelUIController.ChangeBossHpRatio);
        }
        _currentStage.Spawn(_maxStageCount, _maxStageCount - _stageQueue.Count);
    }

    BaseStage ReturnRandomMobStage(List<MobStage> stageObjects)
    {
        int randomRange = UnityEngine.Random.Range(0, stageObjects.Count); // 3, 4, 5, 6, 7
        return stageObjects[randomRange];
    }

    public void CreateRandomStage(GameMode.Level level)
    {
        ILevel levelMap = _inGameFactory.GetFactory(InGameFactory.Type.Level).Create(level);

        BaseStage startStage = levelMap.StartStage;
        Vector3 entryPos = startStage.ReturnEntryPosition();

        Player player = _playerSpawner.Spawn();
        player.transform.position = entryPos;

        levelMap.StartStage.Initialize(this, _addressableHandler, _inGameFactory);
        levelMap.BonusStage.Initialize(this, _addressableHandler, _inGameFactory);

        levelMap.BossStage.Initialize(this, _addressableHandler, _inGameFactory);
        levelMap.BossStage.AddPlayer(player);

        for (int i = 0; i < levelMap.MobStages.Count; i++)
        {
            levelMap.MobStages[i].Initialize(this, _addressableHandler, _inGameFactory);
            levelMap.MobStages[i].AddPlayer(player);
        }

        BaseStage storedBattleStage = null;
        _stageQueue.Enqueue(startStage);

        for (int i = 1; i < _maxStageCount; i++)
        {
            if(i == _maxStageCount - 1)
            {
                BaseStage bossStage = levelMap.BossStage;
                _stageQueue.Enqueue(bossStage);
            }
            else if((i + 1) % _bonusStageGap == 0)
            {
                BaseStage bonusStage = levelMap.BonusStage;
                _stageQueue.Enqueue(bonusStage);
            }
            else
            {
                BaseStage battleStage = ReturnRandomMobStage(levelMap.MobStages);

                if(storedBattleStage == null)
                {
                    _stageQueue.Enqueue(battleStage);
                    storedBattleStage = battleStage;
                }
                else
                {
                    while (storedBattleStage == battleStage) battleStage = ReturnRandomMobStage(levelMap.MobStages);

                    _stageQueue.Enqueue(battleStage);
                    storedBattleStage = battleStage;
                }
            }
        }

        _currentStage = _stageQueue.Dequeue();
        _currentStage.Spawn(_maxStageCount, _maxStageCount - _stageQueue.Count);
        _levelUIController.AddStageCount(1);
    }
}
