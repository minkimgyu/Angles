using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseBuff : IUpgradable
{
    public enum Type
    {
        TotalDamage,
        TotalCooltime,

        ShootingDuration,
        ShootingChargeDuration,

        DashSpeed,
        DashChargeDuration,
    }

    public enum Name
    {
        TotalDamage,
        TotalCooltime,
        Shooting,
        Dash
    }

    protected Dictionary<Type, BuffCommand> _buffCommands;
    public BaseBuff() 
    {
        _buffHistories = new List<BuffCommand>();
    }

    protected List<BuffCommand> _buffHistories;

    int _maxUpgradePoint;
    public int MaxUpgradePoint { get { return _maxUpgradePoint; } }

    int _upgradePoint;
    public int UpgradePoint { get { return _upgradePoint; } }

    public abstract void Initialize(Dictionary<Type, BuffCommand> buffCommands);
    public virtual void OnAdd()
    {
        ApplyBuff();
    }

    protected virtual void OnUpgradeRequested() 
    {
        ApplyBuff();
    }

    protected abstract void ApplyBuff();

    public virtual void OnRemove()
    {
        for (int i = 0; i < _buffHistories.Count; i++)
        {
            _buffHistories[i].UnDo();
        }
    }

    public bool CanUpgrade() { return _upgradePoint < _maxUpgradePoint; }


    public virtual void Upgrade(int step)
    {
        _upgradePoint = step;
        OnUpgradeRequested();
    }

    public virtual void Upgrade()
    {
        _upgradePoint++;
        OnUpgradeRequested();
    }
}