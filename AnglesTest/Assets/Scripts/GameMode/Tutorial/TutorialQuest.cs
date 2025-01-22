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
    public Action QuestStartEvent { get; set; } // ����Ʈ�� ���۵� �� �̺�Ʈ ���������
    public Action QuestEndEvent { get; set; } // ����Ʈ�� �Ϸ�� �� �̺�Ʈ ���������

    public TutorialQuest(Action QuestStartEvent, Action QuestEndEvent)
    {
        this.QuestStartEvent = QuestStartEvent;
        this.QuestEndEvent = QuestEndEvent;
        ClearConditionEvent = new Dictionary<Type, Action>();
    }

    public enum Type
    {
        Move, // �÷��̾� �̵� �� ȣ��
        Shooting, // �÷��̾� ���� �� ȣ��
        Collision, // �÷��̾� ���� �� �浹 �� ȣ��
        CancelShooting, // �÷��̾� ���� ĵ�� �� ȣ��
        GetSkill, // �÷��̾� ��ų ȹ�� �� ����
        ClearStage, // �������� Ŭ���� �� ����
        EnterPotal // �÷��̾ ��Ż ���� �� ����
    }

    public void ClearEvent()
    {
        foreach (Type type in Enum.GetValues(typeof(Type)))
        {
            ClearConditionEvent[type] = null; // ��� Action�� null�� ����
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