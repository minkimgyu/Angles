using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveComponent : MonoBehaviour
{
    Rigidbody2D _rigid;

    bool _applyMovement = true; // ������ ���� ����
    public bool ApplyMovement { set { _applyMovement = value; } }

    bool _applyDirection = true; // ȸ�� ���� ����
    public bool ApplyDirection { set { _applyDirection = value; } }

    float _moveSpeedRatio = 1f; // �ӵ� ����
    public float MoveSpeedRatio { set { _moveSpeedRatio = value; } }

    public void Initialize()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _applyMovement = true;
        _applyDirection = true;
    }

    public void ResetVelocity()
    {
        _rigid.velocity = Vector2.zero;
    }

    public void Stop()
    {
        if (_applyMovement == false) return;

        ResetVelocity();
        FaceDirection(Vector2.right);
    }

    public void FreezeRotation(bool freeze)
    {
        if(freeze) _rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        else _rigid.constraints = RigidbodyConstraints2D.None;
    }

    public void FreezePosition(bool freeze)
    {
        if (freeze) _rigid.constraints = RigidbodyConstraints2D.FreezePosition;
        else _rigid.constraints = RigidbodyConstraints2D.None;
    }

    public void Move(Vector2 direction, float speed)
    {
        if (_applyMovement == false) return;

        _rigid.velocity = direction * speed * _moveSpeedRatio;
        if(_applyDirection == true) FaceDirection(direction, speed);
    }

    public void Move(Vector2 pos)
    {
        if (_applyMovement == false) return;

        _rigid.MovePosition(pos);
    }

    public void RotateDirection(float rotationSpeed, float speedMultiplier)
    {
        _rigid.angularVelocity = rotationSpeed; // 1. z���� �������� ���ӵ� ���� (ȸ�� �ӵ�)
        Vector2 currentVelocity = _rigid.velocity; // 2. ���� �ӵ� ���͸� ��������

        Vector2 newDirection = transform.right;  // 3. ���� ȸ���� ������Ʈ�� ������ ������ �������� (transform.right)
        _rigid.velocity = newDirection * currentVelocity.magnitude * speedMultiplier; // 4. ���ο� �������� �ӵ� ���͸� ���� (�ӵ� ũ��� �״�� ����)
    }

    public void FaceDirection(Vector2 direction)
    {
        // ���Ϳ��� ���� ���ϱ�
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _rigid.MoveRotation(angle);
    }

    float _storedAngle;

    public void FaceDirection(Vector2 direction, float speed)
    {
        // ���Ϳ��� ���� ���ϱ�
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        float angleDifference = Mathf.DeltaAngle(_storedAngle, angle);
        _storedAngle = Mathf.Lerp(_storedAngle, _storedAngle + angleDifference, Time.fixedDeltaTime * speed);

        _rigid.MoveRotation(_storedAngle);
    }

    public void AddForce(Vector2 direction, float speed)
    {
        _rigid.AddForce(direction * speed, ForceMode2D.Impulse);
    }

    public void AddForce(Vector2 direction, float speed, ForceMode2D forceMode)
    {
        _rigid.AddForce(direction * speed, forceMode);
    }
}
