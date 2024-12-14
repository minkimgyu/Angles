using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleEnemy : BasicMob
{
    [SerializeField] DamageableTargetCaptureComponent _skillTargetCaptureComponent;

    public override void ResetData(RectangleData data)
    {
        base.ResetData(data);
        _size = data._size;
        _targetType = data._targetType;
        _moveSpeed = data._moveSpeed;
        _dropData = data._dropData;

        _gap = 0.5f;
        _destoryEffect = BaseEffect.Name.RectangleDestroyEffect;
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
