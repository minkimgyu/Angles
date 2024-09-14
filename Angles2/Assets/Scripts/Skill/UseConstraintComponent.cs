using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Unity.VisualScripting.Antlr3.Runtime;

abstract public class UseConstraintComponent
{
    public abstract bool CanUse();
    public virtual void OnUpdate() { }
    public virtual void Use() { }

    public virtual void AddViewEvent(Action<float, int, bool> viewEvent) { }
    public virtual void RemoveViewEvent(Action<float, int, bool> viewEvent) { }

    public virtual int GetStackCount() { return default; }
    public virtual float GetProbability() { return default; }
    public virtual void SetMaxStackCount(int stackCount) { }
    public virtual void SetCoolTime(float coolTime) { }
}

public class NoConstraintComponent : UseConstraintComponent
{
    public override bool CanUse() { return true; }
}

public class CooltimeConstraint : UseConstraintComponent
{
    public override int GetStackCount() { return _stackCount; }
    public override void SetMaxStackCount(int maxStackCount) { _maxStackCount = maxStackCount; }
    public override void SetCoolTime(float coolTime) { _coolTime = coolTime; }

    protected int _maxStackCount;
    protected int _stackCount;
    protected Timer _cooltimer;
    protected float _coolTime;

    protected bool _showStackCount;
    Action<float, int, bool> ViewerEvent;

    public CooltimeConstraint(int maxStackCount, float coolTime)
    {
        _showStackCount = true;
        _coolTime = coolTime;
        _maxStackCount = maxStackCount;
        if (_maxStackCount == 1) _showStackCount = false;

        _stackCount = 1;
        _cooltimer = new Timer();
    }

    public override void AddViewEvent(Action<float, int, bool> ViewerEvent) { this.ViewerEvent += ViewerEvent; }
    public override void RemoveViewEvent(Action<float, int, bool> ViewerEvent) { this.ViewerEvent -= ViewerEvent; }

    public override bool CanUse() { return _stackCount > 0; }
    public override void Use() { _stackCount -= 1; }

    public override void OnUpdate()
    {
        switch (_cooltimer.CurrentState)
        {
            case Timer.State.Ready:
                if (_stackCount >= _maxStackCount) return;

                _cooltimer.Start(_coolTime);
                break;
            case Timer.State.Running:
                ViewerEvent?.Invoke(1 - _cooltimer.Ratio, _stackCount, _showStackCount);
                break;
            case Timer.State.Finish:
                _cooltimer.Reset();
                _stackCount++;

                if (_stackCount >= _maxStackCount)
                {
                    ViewerEvent?.Invoke(0, _stackCount, _showStackCount);
                    return;
                }

                _cooltimer.Start(_coolTime);
                break;
            default:
                break;
        }
    }
}

public class RandomConstraintComponent : UseConstraintComponent
{
    float _probability;
    public override float GetProbability() { return _probability; }

    public RandomConstraintComponent(float probability)
    {
        _probability = probability;
    }

    public override bool CanUse()
    {
        float random = Random.Range(0.0f, 1.0f);
        return random <= _probability;
    }
}