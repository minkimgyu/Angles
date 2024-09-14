using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveComponent : MonoBehaviour
{
    Rigidbody2D _rigid;
    bool _applyDirection = true;
    public bool ApplyDirection { get { return _applyDirection; } set { _applyDirection = value; } }

    public void Initialize()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _applyDirection = true;
    }

    public void Stop()
    {
        _rigid.velocity = Vector2.zero;
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
        _rigid.velocity = direction * speed;
        if(_applyDirection == true) FaceDirection(direction, speed);
    }

    public void Move(Vector2 pos)
    {
        _rigid.MovePosition(pos);
    }

    public void FaceDirection(Vector2 direction)
    {
        // 벡터에서 각도 구하기
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _rigid.MoveRotation(angle);
    }

    public void FaceDirection(Vector2 direction, float speed)
    {
        Vector2 foward = transform.forward;
        foward = Vector2.Lerp(foward, direction, Time.fixedDeltaTime * speed);

        // 벡터에서 각도 구하기
        float angle = Mathf.Atan2(foward.y, foward.x) * Mathf.Rad2Deg;
        _rigid.MoveRotation(angle);
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
