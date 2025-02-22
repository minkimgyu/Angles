using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhombus : BaseEnemy, ITrackable
{
    [SerializeField] DamageableTargetCaptureComponent _meleeSkillTargetCaptureComponent;
    [SerializeField] TargetCaptureComponent _rangeSkillTargetCaptureComponent;

    [SerializeField] Transform _bottomPoint;
    public override Vector2 BottomPoint => _bottomPoint.localPosition;

    RhombusData _data;

    public override void InjectData(RhombusData data, DropData dropData)
    {
        base.InjectData(data, dropData);
        _data = data;
    }

    public void InjectTarget(ITarget target)
    {
        _moveStrategy.InjectTarget(target);
    }

    public void InjectPathfindEvent(Func<Vector2, Vector2, Size, List<Vector2>> FindPath)
    {
        _moveStrategy.InjectPathfindEvent(FindPath);
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

        _meleeSkillTargetCaptureComponent.Initialize(OnEnter, OnExit);
        _rangeSkillTargetCaptureComponent.Initialize(OnEnter, OnExit);
    }

    void OnEnter(ITarget target)
    {
        _skillController.OnCaptureEnter(target);
    }

    void OnExit(ITarget target)
    {
        _skillController.OnCaptureExit(target);
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
