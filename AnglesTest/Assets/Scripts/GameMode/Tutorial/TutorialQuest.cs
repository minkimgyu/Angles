using System;
using Newtonsoft.Json;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public interface ITutorialInjector
{
    void InjectTutorialEvent(Action event1) { }
    void InjectTutorialEvent(Action event1, Action event2, Action event3) { }
}

[Serializable]
abstract public class TutorialQuest
{
    public Action QuestStartEvent { get; set; } // 퀘스트가 시작될 때 이벤트 실행시켜줌
    public Action QuestEndEvent { get; set; } // 퀘스트가 완료될 때 이벤트 실행시켜줌

    public TutorialQuest(Action QuestStartEvent, Action QuestEndEvent)
    {
        this.QuestStartEvent = QuestStartEvent;
        this.QuestEndEvent = QuestEndEvent;
        ClearConditionEvent = new Dictionary<Type, Action>();
    }

    public enum Type
    {
        Move, // 플레이어 이동 시 호출
        Shooting, // 플레이어 슈팅 시 호출
        Collision, // 플레이어 슈팅 중 충돌 시 호출
        CancelShooting, // 플레이어 슈팅 캔슬 시 호출
        GetSkill, // 플레이어 스킬 획득 시 실행
        ClearStage, // 스테이지 클리어 시 실행
        EnterPotal // 플레이어가 포탈 진입 시 실행
    }

    public void ClearEvent()
    {
        foreach (Type type in Enum.GetValues(typeof(Type)))
        {
            ClearConditionEvent[type] = null; // 모든 Action을 null로 변경
        }
    }

    public Dictionary<Type, Action> ClearConditionEvent { get; protected set; }
}

public class PlayerMoveQuest : TutorialQuest
{
    public PlayerMoveQuest(Action QuestStartEvent, Action QuestEndEvent) 
        : base(QuestStartEvent, QuestEndEvent) 
    {
        ClearConditionEvent[Type.Move] = () => QuestEndEvent?.Invoke();
    }
}

public class PlayerShootingQuest : TutorialQuest
{
    public PlayerShootingQuest(Action QuestStartEvent, Action QuestEndEvent)
        : base(QuestStartEvent, QuestEndEvent) 
    {
        ClearConditionEvent[Type.Shooting] = () => QuestEndEvent?.Invoke();
    }
}

public class PlayerCollisionQuest : TutorialQuest
{
    public PlayerCollisionQuest(Action QuestStartEvent, Action QuestEndEvent)
        : base(QuestStartEvent, QuestEndEvent)
    {
        ClearConditionEvent[Type.Collision] = () => QuestEndEvent?.Invoke();
    }
}

public class PlayerShootingCancelQuest : TutorialQuest
{
    public PlayerShootingCancelQuest(Action QuestStartEvent, Action QuestEndEvent)
        : base(QuestStartEvent, QuestEndEvent) 
    {
        ClearConditionEvent[Type.CancelShooting] = () => QuestEndEvent?.Invoke();
    }
}

public class PlayerGetSkillQuest : TutorialQuest
{
    public PlayerGetSkillQuest(Action QuestStartEvent, Action QuestEndEvent)
        : base(QuestStartEvent, QuestEndEvent) 
    {
        ClearConditionEvent[Type.GetSkill] = () => QuestEndEvent?.Invoke();
    }
}

public class StageClearQuest : TutorialQuest
{
    public StageClearQuest(Action QuestStartEvent, Action QuestEndEvent)
        : base(QuestStartEvent, QuestEndEvent) 
    {
        ClearConditionEvent[Type.ClearStage] = () => QuestEndEvent?.Invoke();
    }
}

public class EnterPotalQuest : TutorialQuest
{
    public EnterPotalQuest(Action QuestStartEvent, Action QuestEndEvent)
        : base(QuestStartEvent, QuestEndEvent)
    {
        ClearConditionEvent[Type.EnterPotal] = () => QuestEndEvent?.Invoke();
    }
}