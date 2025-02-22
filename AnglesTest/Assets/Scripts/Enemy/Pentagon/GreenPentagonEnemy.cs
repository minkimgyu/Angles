using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenPentagonEnemy : BaseEnemy
{
    [SerializeField] TargetCaptureComponent _skillTargetCaptureComponent;
    GreenPentagonData _data;

    float _stopDuration;
    float _rushDuration;

    public override void InjectData(GreenPentagonData data, DropData dropData)
    {
        base.InjectData(data, dropData);
        _data = data;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _moveStrategy.OnCollision(collision);
    }

    void OnCollision(Collision2D collision)
    {
        _skillController.OnReflect(collision.gameObject, collision.contacts[0].point);
    }

    public override void Initialize()
    {
        base.Initialize();
        MoveComponent moveComponent = GetComponent<MoveComponent>();
        moveComponent.Initialize();

        _moveStrategy = new RushStrategy(
             moveComponent,
             transform,
             _data.StopDuration,
             _data.RushDuration,
             _data.MoveSpeed,
             OnCollision
        );
    }
}
