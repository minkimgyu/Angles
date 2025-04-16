using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//public interface IAttackStat
//{
//    public DamageableData DamageableData { get; set; }
//}

//public interface ILifetimeStat
//{
//    public float Lifetime { get; set; }
//}

public interface ILifetimeStrategy
{
    void OnActivate();
    void OnUpdate();
    void ChangeLifetime(float lifeTime) { }
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
    float _lifeTime;

    public ChangeableLifeTimeStrategy(Action OnLifetimeOver)
    {
        _timer = new Timer();
        _lifeTime = 0;
        this.OnLifetimeOver = OnLifetimeOver;
    }

    public void ChangeLifetime(float lifeTime) 
    {
        _lifeTime = lifeTime;
    }

    public void OnActivate()
    {
        _timer.Start(_lifeTime);
    }

    public void OnUpdate()
    {
        if (_timer.CurrentState != Timer.State.Finish) return;

        OnLifetimeOver?.Invoke();
        _timer.Reset();
    }
}