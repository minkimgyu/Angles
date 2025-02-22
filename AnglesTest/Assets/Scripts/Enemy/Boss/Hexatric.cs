using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexatric : BaseEnemy, ITrackable
{
    [SerializeField] TargetCaptureComponent _rangeSkillTargetCaptureComponent;

    [SerializeField] Transform _bottomPoint;
    public override Vector2 BottomPoint => _bottomPoint.localPosition;

    HexatricData _data;

    public override void InjectData(HexatricData data, DropData dropData)
    {
        base.InjectData(data, dropData);
        _data = data;
    }

    public void InjectTarget(ITarget target)
    {
        _moveStrategy.InjectTarget(target);
    }

    public void InjectPathfindEvent(Func<Vector2, Vector2, Size, List<Vector2>> FindPath)
    {
        _moveStrategy.InjectPathfindEvent(FindPath);
    }

    public override void Initialize()
    {
        base.Initialize();

        MoveComponent moveComponent = GetComponent<MoveComponent>();
        TrackComponent trackComponent = GetComponent<TrackComponent>();

        moveComponent.Initialize();
        trackComponent.Initialize(transform, _data.Size);

        _moveStrategy = new TrackStrategy(
            transform,
            moveComponent,
            trackComponent,
            _data.MoveSpeed
        );

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
