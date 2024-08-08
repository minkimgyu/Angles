using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleEnemy : MeleeEnemy
{
    DamageableTargetCaptureComponent _damageableTargetCaptureComponent;

    public override void ResetData(RectangleData data)
    {
        _maxHp = data._maxHp;
        _targetType = data._targetType;
        _moveSpeed = data._moveSpeed;
        _skillNames = data._skillNames;
        _dropData = data._dropData;

        _offsetFromCenter = 0.5f;

        _destoryEffect = BaseEffect.Name.RectangleDestroyEffect;
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
