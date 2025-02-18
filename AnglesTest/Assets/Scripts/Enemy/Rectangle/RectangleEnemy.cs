using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleEnemy : TrackableEnemy
{
    [SerializeField] DamageableTargetCaptureComponent _skillTargetCaptureComponent;

    public override void ResetData(RectangleData data, DropData dropData)
    {
        base.ResetData(data, dropData);
        _size = data.Size;
        _targetType = data.TargetType;
        _moveSpeed = data.MoveSpeed;
        _dropData = dropData;

        _gap = 0.5f;
        _destoryEffect = BaseEffect.Name.RectangleDestroyEffect;
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
