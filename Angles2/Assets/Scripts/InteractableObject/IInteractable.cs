using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct InteractEnterData
{
    public InteractEnterData(IFollowable followable)
    {
        _followable = followable;
    }

    IFollowable _followable;
    public IFollowable Followable { get { return _followable; } }
}

public struct InteractData
{
    public InteractData(Func<List<SkillUpgradeData>> returnSkillUpgradeDatas, Action<BaseSkill.Name> addSkill, Action<Vector2> resetPosition)
    {
        _returnSkillUpgradeDatas = returnSkillUpgradeDatas;
        _addSkill = addSkill;
        _resetPosition = resetPosition;
    }

    Action<BaseSkill.Name> _addSkill;
    public Action<BaseSkill.Name> AddSkill { get { return _addSkill; } }

    Func<List<SkillUpgradeData>> _returnSkillUpgradeDatas;
    public Func<List<SkillUpgradeData>> ReturnSkillUpgradeDatas { get { return _returnSkillUpgradeDatas; } }

    Action<Vector2> _resetPosition;
    public Action<Vector2> ResetPosition { get { return _resetPosition; } }
}

public struct InteractExitData
{

}

public interface IInteractable
{
    public enum Name
    {
        Coin,
        CardTable
    }

    void OnInteractEnter(InteractEnterData data);
    void OnInteract(InteractData data);
    void OnInteractExit(InteractExitData data);
}
