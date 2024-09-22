using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILifetimeStat
{
    public float Lifetime { get; set; }
}

abstract public class BaseLifetimeComponent
{
    public virtual void Activate() { }
    public virtual bool IsFinish() { return false; }
}

public class NoLifetimeComponent : BaseLifetimeComponent
{
}

public class LifetimeComponent : BaseLifetimeComponent
{
    Timer _timer;
    ILifetimeStat _lifetimeData;

    public LifetimeComponent(ILifetimeStat lifetimeData)
    {
        _timer = new Timer();
        _lifetimeData = lifetimeData;
    }

    public override void Activate()
    {
        _timer.Start(_lifetimeData.Lifetime);
    }

    public override bool IsFinish()
    {
        return _timer.CurrentState == Timer.State.Finish;
    }
}