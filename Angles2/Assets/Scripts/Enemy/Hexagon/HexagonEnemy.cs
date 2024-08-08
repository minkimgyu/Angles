using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonEnemy : RangedEnemy
{
    TargetCaptureComponent _targetCaptureComponent;

    public override void ResetData(HexagonData data)
    {
        _maxHp = data._maxHp;
        _targetType = data._targetType;
        _moveSpeed = data._moveSpeed;
        _skillNames = data._skillNames;
        _dropData = data._dropData;

        _offsetFromCenter = 1.0f;

        _stopDistance = data._stopDistance;
        _gap = data._gap;
        _destoryEffect = BaseEffect.Name.HexagonDestroyEffect;
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
