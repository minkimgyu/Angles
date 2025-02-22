using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleEnemy : BaseEnemy, ITrackable
{
    [SerializeField] DamageableTargetCaptureComponent _skillTargetCaptureComponent;
    TriangleData _data;

    public override void InjectData(TriangleData data, DropData dropData)
    {
        base.InjectData(data, dropData);
        _data = data;
    }

    public void InjectPathfindEvent(Func<Vector2, Vector2, Size, List<Vector2>> FindPath)
    {
        _moveStrategy.InjectPathfindEvent(FindPath);
    }

    public void InjectTarget(ITarget target)
    {
        _moveStrategy.InjectTarget(target);
    }

    public override void Initialize()
    {
        base.Initialize();

        MoveComponent moveComponent = GetComponent<MoveComponent>();
        TrackComponent trackComponent = GetComponent<TrackComponent>();
        moveComponent.Initialize();
        trackComponent.Initialize(transform, _data.Size);

        _moveStrategy = new TrackStrategy(
            transform,
            moveComponent,
            trackComponent,
            _data.MoveSpeed
        );

        _skillTargetCaptureComponent.Initialize(OnEnter, OnExit);
    }

    void OnEnter(ITarget target, IDamageable damageable)
    {
        _skillController.OnCaptureEnter(target, damageable);
    }

    void OnExit(ITarget target, IDamageable damageable)
    {
        _skillController.OnCaptureExit(target, damageable);
    }
}
