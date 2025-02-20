using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhombus : TrackableEnemy
{
    [SerializeField] DamageableTargetCaptureComponent _meleeSkillTargetCaptureComponent;
    [SerializeField] TargetCaptureComponent _rangeSkillTargetCaptureComponent;

    [SerializeField] Transform _bottomPoint;
    public override Vector2 BottomPoint => _bottomPoint.localPosition;

    public override void ResetData(RhombusData data, DropData dropData)
    {
        base.ResetData(data, dropData);
        _size = data.Size;
        _targetType = data.TargetType;
        _moveSpeed = data.MoveSpeed;
        _dropData = dropData;

        _stopDistance = data.StopDistance;
        _gap = data.Gap;

        _destoryEffect = BaseEffect.Name.HexagonDestroyEffect;
    }

    public override void InitializeFSM(Func<Vector2, Vector2, Size, List<Vector2>> FindPath)
    {
        _moveStrategy = new TrackComponent(
             _moveComponent,
             transform,
             _size,
             _moveSpeed,
             _stopDistance,
             _gap,
             FindPath
        );
    }

    public override void Initialize()
    {
        base.Initialize();
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
