using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleEnemy : MeleeEnemy
{
    DamageableTargetCaptureComponent _damageableTargetCaptureComponent;

    public override void ResetData(TriangleData data)
    {
        _maxHp = data._maxHp;
        _targetType = data._targetType;
        _moveSpeed = data._moveSpeed;
        _skillNames = data._skillNames;

        _destoryEffect = BaseEffect.Name.TriangleDestroy;
    }

    public override void Initialize()
    {
        base.Initialize();

        _damageableTargetCaptureComponent = GetComponentInChildren<DamageableTargetCaptureComponent>();
        _damageableTargetCaptureComponent.Initialize(OnEnter, OnExit);
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
