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
    void OnActivate();
    void OnUpdate();
}

public class NoLifetimeStrategy : ILifetimeStrategy
{
    public void OnActivate() { }
    public void OnUpdate() { }
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

    public void OnActivate()
    {
        _timer.Start(_lifetimeData.Lifetime);
    }

    public void OnUpdate()
    {
        if (_timer.CurrentState != Timer.State.Finish) return;

        OnLifetimeOver?.Invoke();
        _timer.Reset();
    }
}