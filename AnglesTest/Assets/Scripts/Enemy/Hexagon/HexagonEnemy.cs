using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonEnemy : BasicMob
{
    [SerializeField] TargetCaptureComponent _skillTargetCaptureComponent;

    public override void ResetData(HexagonData data, DropData dropData)
    {
        base.ResetData(data, dropData);
        _size = data.Size;
        _targetType = data.TargetType;
        _moveSpeed = data.MoveSpeed;
        _dropData = dropData;

        _stopDistance = data.StopDistance;
        _gap = data.Gap;
        _destoryEffect = BaseEffect.Name.HexagonDestroyEffect;
    }

    public override void Initialize()
    {
        base.Initialize();
        _skillTargetCaptureComponent.Initialize(OnEnter, OnExit);
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
