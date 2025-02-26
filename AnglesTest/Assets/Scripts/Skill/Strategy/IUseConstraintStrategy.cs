using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public interface IUseConstraintStrategy
{
    public abstract bool CanUse();
    public virtual void OnUpdate() { }
    public virtual void Use() { }

    public virtual void AddViewEvent(Action<float, int, bool> viewEvent) { }
    public virtual void RemoveViewEvent(Action<float, int, bool> viewEvent) { }
}

public class NoConstraintStrategy : IUseConstraintStrategy
{
    public bool CanUse() { return true; }
}

public class CooltimeConstraintStrategy : IUseConstraintStrategy
{
    protected int _stackCount;
    protected Timer _cooltimer;

    protected bool _showStackCount;
    Action<float, int, bool> ViewerEvent;

    CooltimeSkillData _cooltimeSkillData;
    IUpgradeableSkillData _upgradeableRatio;

    public CooltimeConstraintStrategy(CooltimeSkillData cooltimeSkillData, IUpgradeableSkillData upgradeableRatio)
    {
        _showStackCount = true;
        _cooltimeSkillData = cooltimeSkillData;
        _upgradeableRatio = upgradeableRatio;

        _stackCount = 1;
        _cooltimer = new Timer();
    }

    public void AddViewEvent(Action<float, int, bool> ViewerEvent) { this.ViewerEvent += ViewerEvent; }
    public void RemoveViewEvent(Action<float, int, bool> ViewerEvent) { this.ViewerEvent -= ViewerEvent; }

    public bool CanUse() { return _stackCount > 0; }
    public void Use() { _stackCount -= 1; }

    public void OnUpdate()
    {
        bool showStack = true;
        switch (_cooltimer.CurrentState)
        {
            case Timer.State.Ready:
                if (_stackCount >= _cooltimeSkillData.MaxStackCount) return;

                _cooltimer.Start(_cooltimeSkillData.CoolTime * _upgradeableRatio.TotalCooltimeRatio);
                break;
            case Timer.State.Running:
                ViewerEvent?.Invoke(1 - _cooltimer.Ratio, _stackCount, showStack);
                break;
            case Timer.State.Finish:
                _cooltimer.Reset();
                _stackCount++;

                if (_stackCount >= _cooltimeSkillData.MaxStackCount)
                {
                    ViewerEvent?.Invoke(0, _stackCount, showStack);
                    return;
                }

                _cooltimer.Start(_cooltimeSkillData.CoolTime * _upgradeableRatio.TotalCooltimeRatio);
                break;
            default:
                break;
        }
    }
}

public class RandomConstraintStrategy : IUseConstraintStrategy
{
    RandomSkillData _randomSkillData;
    IUpgradeableSkillData _upgradeableRatio;

    public RandomConstraintStrategy(RandomSkillData randomSkillData, IUpgradeableSkillData upgradeableRatio)
    {
        _randomSkillData = randomSkillData;
        _upgradeableRatio = upgradeableRatio;
    }

    public bool CanUse()
    {
        float random = Random.Range(0.0f, 1.0f);
        return random <= _randomSkillData.Probability * _upgradeableRatio.TotalRandomRatio;

        // 0.7 * 1.2
        // 0.7 * 0.8
    }
}