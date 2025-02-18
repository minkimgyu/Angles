using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefractionBullet : Bullet
{
    const float _rotationSpeed = 70f; // 회전 속도 (각속도, 도/초)
    const float _speedMultiplier = 1.5f; // 속도 배율

    protected override void Update()
    {
        base.Update();
        _moveComponent.RotateDirection(_rotationSpeed, _speedMultiplier);
    }
}
