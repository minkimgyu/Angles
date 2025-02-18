using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefractionBullet : Bullet
{
    const float _rotationSpeed = 70f; // ȸ�� �ӵ� (���ӵ�, ��/��)
    const float _speedMultiplier = 1.5f; // �ӵ� ����

    protected override void Update()
    {
        base.Update();
        _moveComponent.RotateDirection(_rotationSpeed, _speedMultiplier);
    }
}
