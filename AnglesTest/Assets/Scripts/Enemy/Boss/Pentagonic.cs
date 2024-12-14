using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pentagonic : BasicMob
{
    [SerializeField] TargetCaptureComponent _rangeSkillTargetCaptureComponent;

    public override void ResetData(PentagonicData data)
    {
        base.ResetData(data);
        _size = data._size;
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
}
