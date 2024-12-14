using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefractionBullet : Bullet
{
    public float _rotationSpeed = 70f; // ȸ�� �ӵ� (���ӵ�, ��/��)
    public float _speedMultiplier = 1.5f; // �ӵ� ����

    private Rigidbody2D rb;

    protected override void Update()
    {
        base.Update();
        _moveComponent.RotateDirection(_rotationSpeed, _speedMultiplier);
    }
}
