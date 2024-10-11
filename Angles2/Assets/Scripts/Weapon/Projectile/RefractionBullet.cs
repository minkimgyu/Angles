using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefractionBullet : Bullet
{
    public float _rotationSpeed = 70f; // 회전 속도 (각속도, 도/초)
    public float _speedMultiplier = 1.5f; // 속도 배율

    private Rigidbody2D rb;

    protected override void Update()
    {
        base.Update();
        _moveComponent.RotateDirection(_rotationSpeed, _speedMultiplier);
    }
}
