using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pentagonic : BasicMob
{
    [SerializeField] TargetCaptureComponent _rangeSkillTargetCaptureComponent;

    public override void ResetData(PentagonicData data, DropData dropData)
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
