using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface ILifetimeStat
{
    public float Lifetime { get; set; }
}

abstract public class BaseLifetimeComponent
{
    public virtual void Activate() { }
    public virtual void CheckFinish() { }
}

public class NoLifetimeComponent : BaseLifetimeComponent
{
}

public class LifetimeComponent : BaseLifetimeComponent
{
    Action OnLifetimeOver;
    Timer _timer;
    ILifetimeStat _lifetimeData;

    public LifetimeComponent(ILifetimeStat lifetimeData, Action OnLifetimeOver)
    {
        _timer = new Timer();
        _lifetimeData = lifetimeData;
        this.OnLifetimeOver = OnLifetimeOver;
    }

    public override void Activate()
    {
        _timer.Start(_lifetimeData.Lifetime);
    }

    public override void CheckFinish()
    {
        if (_timer.CurrentState != Timer.State.Finish) return;

        OnLifetimeOver?.Invoke();
        _timer.Reset();
    }
}