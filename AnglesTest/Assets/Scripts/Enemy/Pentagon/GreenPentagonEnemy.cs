using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenPentagonEnemy : BaseEnemy
{
    [SerializeField] TargetCaptureComponent _skillTargetCaptureComponent;

    float _stopDuration;
    float _rushDuration;

    public override void ResetData(GreenPentagonData data, DropData dropData)
    {
        base.ResetData(data, dropData);
        _size = data.Size;
        _targetType = data.TargetType;
        _moveSpeed = data.MoveSpeed;
        _dropData = dropData;

        _stopDuration = data.StopDuration;
        _rushDuration = data.RushDuration;
        _destoryEffect = BaseEffect.Name.PentagonDestroyEffect;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _moveStrategy.OnCollision(collision);
    }

    void OnCollision(Collision2D collision)
    {
        _skillController.OnReflect(collision.gameObject, collision.contacts[0].point);
    }

    public override void InitializeFSM(Func<Vector2, Vector2, Size, List<Vector2>> FindPath)
    {
        _moveStrategy = new RushComponent(
             _moveComponent,
             transform,
             _stopDuration,
             _rushDuration,
             _moveSpeed,
             OnCollision
        );
    }
}
