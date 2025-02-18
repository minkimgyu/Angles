using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IAttackStat
{
    public DamageableData DamageableData { get; set; }
}

public interface ILifetimeStat
{
    public float Lifetime { get; set; }
}

public interface ILifetimeStrategy
{
    void Activate();
    void CheckFinish();
}

public class NoLifetimeStrategy : ILifetimeStrategy
{
    public void Activate() { }
    public void CheckFinish() { }
}

public class ChangeableLifeTimeStrategy : ILifetimeStrategy
{
    Action OnLifetimeOver;
    Timer _timer;
    ILifetimeStat _lifetimeData;

    public ChangeableLifeTimeStrategy(ILifetimeStat lifetimeData, Action OnLifetimeOver)
    {
        _timer = new Timer();
        _lifetimeData = lifetimeData;
        this.OnLifetimeOver = OnLifetimeOver;
    }

    public void Activate()
    {
        _timer.Start(_lifetimeData.Lifetime);
    }

    public void CheckFinish()
    {
        if (_timer.CurrentState != Timer.State.Finish) return;

        OnLifetimeOver?.Invoke();
        _timer.Reset();
    }
}