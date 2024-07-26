using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooltimeSkill : BaseSkill
{
    protected int _maxStackCount;
    protected int _stackCount;
    protected Timer _cooltimer;
    protected float _coolTime;

    protected bool _showStackCount;

    public CooltimeSkill(int maxUpgradePoint, float coolTime, int maxStackCount) : base(Type.Active, maxUpgradePoint)
    {
        _showStackCount = true;
        _coolTime = coolTime;
        _maxStackCount = maxStackCount;
        if (_maxStackCount == 1) _showStackCount = false;

        _stackCount = 1;
        _cooltimer = new Timer();
    }

    public override void OnUpdate()
    {
        switch (_cooltimer.CurrentState)
        {
            case Timer.State.Ready:
                if (_stackCount >= _maxStackCount) return;

                _cooltimer.Start(_coolTime);
                break;
            case Timer.State.Running:
                ResetViewerValue?.Invoke(1 - _cooltimer.Ratio, _stackCount, _showStackCount);
                break;
            case Timer.State.Finish:
                _cooltimer.Reset();
                _stackCount++;

                if (_stackCount >= _maxStackCount)
                {
                    ResetViewerValue?.Invoke(0, _stackCount, _showStackCount);
                    return;
                }

                _cooltimer.Start(_coolTime);
                break;
            default:
                break;
        }
    }

    public override bool CanUse()
    {
        return _stackCount > 0;
    }
}