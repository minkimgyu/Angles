using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using Skill;

public class TutorialMode : DungeonMode
{
    public struct Events
    {
        public Action MoveStartTutorialEvent { get; private set; }
        public Action ShootingTutorialEvent { get; private set; }
        public Action CollisionTutorialEvent { get; private set; }

        public Action CancelShootingTutorialEvent { get; private set; }
        public Action OnGetSkillTutorialEvent { get; private set; }
        public Action OnStageClearTutorialEvent { get; private set; }
        public Action OnEnterPotalTutorialEvent { get; private set; }

        public Events(
            Action MoveStartTutorialEvent,
            Action ShootingTutorialEvent,
            Action CollisionTutorialEvent,

            Action CancelShootingTutorialEvent,
            Action OnGetSkillTutorialEvent,
            Action OnStageClearTutorialEvent,
            Action OnEnterPotalTutorialEvent)
        {
            this.MoveStartTutorialEvent = MoveStartTutorialEvent;
            this.ShootingTutorialEvent = ShootingTutorialEvent;
            this.CollisionTutorialEvent = CollisionTutorialEvent;

            this.CancelShootingTutorialEvent = CancelShootingTutorialEvent;
            this.OnGetSkillTutorialEvent = OnGetSkillTutorialEvent;
            this.OnStageClearTutorialEvent = OnStageClearTutorialEvent;
            this.OnEnterPotalTutorialEvent = OnEnterPotalTutorialEvent;
        }
    }

    [SerializeField] TutorialLevelUIController _levelUIController;
    [SerializeField] TutorialScriptController _tutorialScriptController;
    
    ILevel _level;

    TutorialQuest _currentQuest;
    Queue<TutorialQuest> _questEventQueue;

    TutorialScriptModel _tutorialScriptModel;

    int _totalTutorialCount;
    int _currentTutorialIndex;

    void Start()
    {
        Initialize(GameMode.Type.Tutorial);
        BuildQueue();

        _currentTutorialIndex = 0;
        _totalTutorialCount = _questEventQueue.Count;

        _tutorialScriptController.Initialize();

        if (_questEventQueue.Count == 0) return;

        _currentQuest = _questEventQueue.Dequeue();
        _currentQuest.QuestStartEvent?.Invoke();
    }

    void OnPlayerMoveStart()
    {
        if (_currentQuest.ClearConditionEvent.ContainsKey(TutorialQuest.Type.Move) == false) return;
        _currentQuest.ClearConditionEvent[TutorialQuest.Type.Move]?.Invoke();
    }

    void OnPlayerShooting()
    {
        if (_currentQuest.ClearConditionEvent.ContainsKey(TutorialQuest.Type.Shooting) == false) return;
        _currentQuest.ClearConditionEvent[TutorialQuest.Type.Shooting]?.Invoke();
    }

    void OnPlayerCollision()
    {
        if (_currentQuest.ClearConditionEvent.ContainsKey(TutorialQuest.Type.Collision) == false) return;
        _currentQuest.ClearConditionEvent[TutorialQuest.Type.Collision]?.Invoke();
    }

    void OnPlayerCancelShooting()
    {
        if (_currentQuest.ClearConditionEvent.ContainsKey(TutorialQuest.Type.CancelShooting) == false) return;
        _currentQuest.ClearConditionEvent[TutorialQuest.Type.CancelShooting]?.Invoke();
    }
    void OnPlayerGetSkill()
    {
        if (_currentQuest.ClearConditionEvent.ContainsKey(TutorialQuest.Type.GetSkill) == false) return;
        _currentQuest.ClearConditionEvent[TutorialQuest.Type.GetSkill]?.Invoke();
    }

    void OnStageClear()
    {
        if (_currentQuest.ClearConditionEvent.ContainsKey(TutorialQuest.Type.ClearStage) == false) return;
        _currentQuest.ClearConditionEvent[TutorialQuest.Type.ClearStage]?.Invoke();
    }

    void OnEnterPotal()
    {
        if (_currentQuest.ClearConditionEvent.ContainsKey(TutorialQuest.Type.EnterPotal) == false) return;
        _currentQuest.ClearConditionEvent[TutorialQuest.Type.EnterPotal]?.Invoke();
    }

    const float _delayAfterQuestComplete = 3.0f;
    const float _delayForNextQuest = 3.0f;

    // 퀘스트 완료 시 호출
    void OnQuestCompleted()
    {
        _currentQuest.ClearEvent();
        _currentTutorialIndex++;

        string completed = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.Completed);

        _levelUIController.ChangeStageCount(_currentTutorialIndex);
        _levelUIController.ShowStageResult(true);
        _levelUIController.ChangeStageResultInfo($"{_currentTutorialIndex} / {_totalTutorialCount} {completed}");

        if (_questEventQueue.Count == 0)
        {
            OnGameClearRequested();
            return;
        }

        DOVirtual.DelayedCall(_delayAfterQuestComplete, () =>
        {
            _tutorialScriptController.ActivateScript(false, 0.5f);

            DOVirtual.DelayedCall(_delayForNextQuest, () =>
            {
                _currentQuest = _questEventQueue.Dequeue();
                _currentQuest.QuestStartEvent?.Invoke();
            });
        });
    }

    void BuildQueue()
    {
        PlayerMoveQuest playerMoveQuest = new PlayerMoveQuest(() => 
        {
            string title = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.MoveQuestTitle);
            string content = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.MoveQuestContent);

            _tutorialScriptController.ActivateScript(true, 0.5f);
            _tutorialScriptController.ChangeScript(title, content);

            _tutorialScriptController.FadeInOutFinger(1, 2); 
        }, OnQuestCompleted);

        PlayerShootingQuest playerShootingQuest = new PlayerShootingQuest(() => 
        {
            string title = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.ShootingQuestTitle);
            string content = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.ShootingQuestContent);

            _tutorialScriptController.ActivateScript(true, 0.5f);
            _tutorialScriptController.ChangeScript(title, content);

            _tutorialScriptController.DragRightFinger(new Vector2(200, -200), 1, 0.5f);
        }, OnQuestCompleted);

        PlayerCollisionQuest playerCollisionQuest = new PlayerCollisionQuest(() =>
        {
            string title = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.CollisionQuestTitle);
            string content = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.CollisionQuestContent);

            _tutorialScriptController.ActivateScript(true, 0.5f);
            _tutorialScriptController.ChangeScript(title, content);

        }, OnQuestCompleted);

        PlayerShootingCancelQuest playerShootingCancelQuest = new PlayerShootingCancelQuest(() => 
        {
            string title = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.CancelShootingQuestTitle);
            string content = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.CancelShootingQuestContent);

            _tutorialScriptController.ActivateScript(true, 0.5f);
            _tutorialScriptController.ChangeScript(title, content);

        }, OnQuestCompleted);

        PlayerGetSkillQuest playerGetSkillQuest = new PlayerGetSkillQuest(() => 
        {
            string title = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.GetSkillQuestTitle);
            string content = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.GetSkillQuestContent);

            _level.TutorialStageLevel.DestroySkillBoxWall();
            _tutorialScriptController.ActivateScript(true, 0.5f);
            _tutorialScriptController.ChangeScript(title, content);

        }, OnQuestCompleted);

        StageClearQuest stageClearQuest = new StageClearQuest(() => 
        {
            string title = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.StageClearQuestTitle);
            string content = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.StageClearQuestContent);

            _tutorialScriptController.ActivateScript(true, 0.5f);
            _tutorialScriptController.ChangeScript(title, content);
            _level.TutorialStageLevel.Spawn();
        }, OnQuestCompleted);

        EnterPotalQuest enterPotalQuest = new EnterPotalQuest(() => 
        {
            string title = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.EnterPotalQuestTitle);
            string content = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.EnterPotalQuestContent);

            _level.TutorialStageLevel.ActivePortal();
            _tutorialScriptController.ActivateScript(true, 0.5f);
            _tutorialScriptController.ChangeScript(title, content);
        }, OnQuestCompleted);

        _questEventQueue = new Queue<TutorialQuest>();
        _questEventQueue.Enqueue(playerMoveQuest);
        _questEventQueue.Enqueue(playerShootingQuest);
        _questEventQueue.Enqueue(playerCollisionQuest);
        _questEventQueue.Enqueue(playerShootingCancelQuest);
        _questEventQueue.Enqueue(playerGetSkillQuest);
        _questEventQueue.Enqueue(stageClearQuest);
        _questEventQueue.Enqueue(enterPotalQuest);
    }

    protected override void InitializeGameStageManager()
    {
        GameState gameState = new GameState(_coinViewer);
        GameStateManager.Instance.Initialize(gameState);
    }

    protected override void OnGameEnd()
    {
        base.OnGameEnd();

        int coinCount = GameStateManager.Instance.ReturnCoin();
        ServiceLocater.ReturnSaveManager().AddCoinCount(coinCount);

        SaveData data = ServiceLocater.ReturnSaveManager().GetSaveData();
        ServiceLocater.ReturnSaveManager().ChangeLevelProgress(data._selectedLevel[Type.Tutorial], 1);
    }

    protected override void InitializeLevel(AddressableHandler addressableHandler, InGameFactory inGameFactory, Level level)
    {
        _level = inGameFactory.GetFactory(InGameFactory.Type.Level).Create(level);
        _level.TutorialStageLevel.transform.position = Vector2.zero;

        _levelUIController.Initialize();

        Events events = new Events(
            OnPlayerMoveStart, 
            OnPlayerShooting,
            OnPlayerCollision,

            OnPlayerCancelShooting,
            OnPlayerGetSkill,
            OnStageClear,
            OnEnterPotal
        );

        _level.TutorialStageLevel.Initialize(this, addressableHandler, inGameFactory, _levelUIController, events);
    }

    protected override BaseFactory ReturnStageFactory(AddressableHandler addressableHandler)
    {
        return new TutorialStageFactory(addressableHandler.LevelAsset, addressableHandler.LevelDesignAsset);
    }

    protected override HashSet<BaseSkill.Name> GetUpgradeableSkills(Database database)
    {
        return database.TutorialUpgradeableSkills;
    }
}
