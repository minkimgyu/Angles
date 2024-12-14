using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveComponent : MonoBehaviour
{
    Rigidbody2D _rigid;

    bool _applyMovement = true; // 움직임 적용 여부
    public bool ApplyMovement { set { _applyMovement = value; } }

    bool _applyDirection = true; // 회전 적용 여부
    public bool ApplyDirection { set { _applyDirection = value; } }

    float _moveSpeedRatio = 1f; // 속도 비율
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
        _rigid.angularVelocity = rotationSpeed; // 1. z축을 기준으로 각속도 설정 (회전 속도)
        Vector2 currentVelocity = _rigid.velocity; // 2. 현재 속도 벡터를 가져오기

        Vector2 newDirection = transform.right;  // 3. 현재 회전된 오브젝트의 오른쪽 방향을 가져오기 (transform.right)
        _rigid.velocity = newDirection * currentVelocity.magnitude * speedMultiplier; // 4. 새로운 방향으로 속도 벡터를 수정 (속도 크기는 그대로 유지)
    }

    public void FaceDirection(Vector2 direction)
    {
        // 벡터에서 각도 구하기
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _rigid.MoveRotation(angle);
    }

    float _storedAngle;

    public void FaceDirection(Vector2 direction, float speed)
    {
        // 벡터에서 각도 구하기
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
