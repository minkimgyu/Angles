using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lombard : TrackableEnemy
{
    [SerializeField] DamageableTargetCaptureComponent _meleeSkillTargetCaptureComponent;
    [SerializeField] TargetCaptureComponent _rangeSkillTargetCaptureComponent;

    public override void ResetData(LombardData data)
    {
        _size = data._size;
        _maxHp = data._maxHp;
        _targetType = data._targetType;
        _moveSpeed = data._moveSpeed;
        _dropData = data._dropData;

        _stopDistance = data._stopDistance;
        _gap = data._gap;

        _destoryEffect = BaseEffect.Name.HexagonDestroyEffect;
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
