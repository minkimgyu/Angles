using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class TrackableEnemy : BaseEnemy, ITrackable
{
    protected List<ITarget.Type> _followableTypes;

    protected float _stopDistance;
    protected float _gap;

    //protected ITarget _followTarget;

    void AddTarget(ITarget target)
    {
        //_followTarget = target;
        _moveStrategy.AddTarget(target);
    }

    public void InjectTarget(ITarget target)
    {
        AddTarget(target);
    }

    public override void Initialize()
    {
        base.Initialize();
        _followableTypes = new List<ITarget.Type> { ITarget.Type.Blue };
    }

    protected override void Update()
    {
        base.Update();

        if (_aliveState == AliveState.Groggy) return;
        _moveStrategy.OnUpdate();
    }

    void FixedUpdate()
    {
        if (_aliveState == AliveState.Groggy) return;
        _moveStrategy.OnFixedUpdate();
    }
}