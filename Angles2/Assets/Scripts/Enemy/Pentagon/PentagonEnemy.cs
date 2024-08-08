using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PentagonEnemy : RangedEnemy
{
    TargetCaptureComponent _targetCaptureComponent;

    public override void ResetData(PentagonData data)
    {
        _maxHp = data._maxHp;
        _targetType = data._targetType;
        _moveSpeed = data._moveSpeed;
        _skillNames = data._skillNames;
        _dropData = data._dropData;

        _offsetFromCenter = 0.7f;

        _stopDistance = data._stopDistance;
        _gap = data._gap;

        _destoryEffect = BaseEffect.Name.PentagonDestroyEffect;
    }

    public override void Initialize()
    {
        base.Initialize();
        _targetCaptureComponent = GetComponentInChildren<TargetCaptureComponent>();
        _targetCaptureComponent.Initialize(OnEnter, OnExit);
    }

    void OnEnter(ITarget target)
    {
        _skillController.OnCaptureEnter(target);
    }

    void OnExit(ITarget target)
    {
        _skillController.OnCaptureExit(target);
    }
}
