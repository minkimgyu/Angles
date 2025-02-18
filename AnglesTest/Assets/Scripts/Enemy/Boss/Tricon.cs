using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tricon : TrackableEnemy
{
    DamageableTargetCaptureComponent _meleeTargetCaptureComponent;

    float _movableDuration;
    float _freezeDuration;

    const float _noSpeed = 0;

    [SerializeField] Transform _bottomPoint;
    public override Vector2 BottomPoint => _bottomPoint.localPosition;

    public override void ResetData(TriconData data, DropData dropData)
    {
        base.ResetData(data, dropData);
        _size = data.Size;
        _targetType = data.TargetType;
        _moveSpeed = data.MoveSpeed;
        _dropData = dropData;

        _stopDistance = data.StopDistance;
        _gap = data.Gap;

        _freezeDuration = data.FreezeDuration;
        _movableDuration = data.MovableDuration;

        _destoryEffect = BaseEffect.Name.HexagonDestroyEffect;

        _meleeTargetCaptureComponent = GetComponentInChildren<DamageableTargetCaptureComponent>();
        _meleeTargetCaptureComponent.Initialize(OnEnter, OnExit);
    }

    public override void InitializeFSM(Func<Vector2, Vector2, Size, List<Vector2>> FindPath)
    {
        _moveStrategy = new FlexibleTrackingComponent(
            _moveComponent,
            transform,
            _size,
            _noSpeed,
            _moveSpeed,
            _stopDistance,
            _gap,
            _freezeDuration,
            _movableDuration,
            FindPath
        );
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
