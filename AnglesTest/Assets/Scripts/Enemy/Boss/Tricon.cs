using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tricon : TrackableEnemy
{
    [SerializeField] DamageableTargetCaptureComponent _meleeSkillTargetCaptureComponent;

    float _movableDuration;
    float _freezeDuration;

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
    }

    public override void InitializeFSM(Func<Vector2, Vector2, Size, List<Vector2>> FindPath)
    {
        _fsm.Initialize(
           new Dictionary<State, BaseState<State>>
           {
               { State.Wandering, new WanderingState(_fsm, _moveComponent, transform, _followableTypes, 3, _moveSpeed, _moveSpeed) },
               { State.Tracking, new FreezeTrackingState(_fsm, _moveComponent, transform, _size, _moveSpeed, _stopDistance, _gap, _freezeDuration, _movableDuration, FindPath) }
           },
           State.Wandering
        );
    }

    public override void Initialize()
    {
        base.Initialize();

        _meleeSkillTargetCaptureComponent.Initialize(OnEnter, OnExit);
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
