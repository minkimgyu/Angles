using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenRectangleEnemy : BaseEnemy, ITrackable
{
    [SerializeField] TargetCaptureComponent _skillTargetCaptureComponent;
    RectangleData _data;

    public override void InjectData(RectangleData data, DropData dropData)
    {
        base.InjectData(data, dropData);
        _data = data;
    }

    public void InjectPathfindEvent(Func<Vector2, Vector2, Size, List<Vector2>> FindPath)
    {
        _moveStrategy.InjectPathfindEvent(FindPath);
    }

    public void InjectTarget(ITarget target)
    {
        _moveStrategy.InjectTarget(target);
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
