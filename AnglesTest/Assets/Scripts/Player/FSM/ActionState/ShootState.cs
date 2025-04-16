using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShootState : State<Player.ActionState>
{
    Action<bool, float> ChangeBodyScale;

    Action<bool> SetInvincible;
    Action<GameObject, Vector2, Vector2> OnReflect;

    float _ratio;

    Timer _timer;
    Transform _myTransform;
    MoveComponent _moveComponent;

    PlayerData _playerData;

    LayerMask _layerMask;

    public ShootState(
        FSM<Player.ActionState> fsm,

        PlayerData playerData,

        Transform myTransform,
        MoveComponent moveComponent,

        Action<bool, float> ChangeBodyScale,
        Action<GameObject, Vector2, Vector2> OnReflect,

        Action<bool> SetInvincible) : base(fsm)
    {
        _playerData = playerData;
        _myTransform = myTransform;
        _moveComponent = moveComponent;

        this.ChangeBodyScale = ChangeBodyScale;
        this.OnReflect = OnReflect;

        _timer = new Timer();
        this.SetInvincible = SetInvincible;

        _layerMask = LayerMask.GetMask("Obstacle", "Target");
    }

    Action ShootingTutorialEvent;
    Action CollisionTutorialEvent;
    Action CancelShootingTutorialEvent;

    public override void InjectTutorialEvent(Action ShootingTutorialEvent, Action CollisionTutorialEvent, Action CancelShootingTutorialEvent)
    {
        this.ShootingTutorialEvent = ShootingTutorialEvent;
        this.CollisionTutorialEvent = CollisionTutorialEvent;
        this.CancelShootingTutorialEvent = CancelShootingTutorialEvent;
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
        ReflectTo(collision.gameObject, collision.contacts[0].point, collision.contacts[0].normal);
    }

    void ReflectTo(GameObject targetObject, Vector3 contactPos, Vector3 contactNormal)
    {
        // collision -> 적, 벽
        //Debug.Log(targetObject.name);
        CollisionTutorialEvent?.Invoke();

        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Bounce, 0.6f);
        OnReflect?.Invoke(targetObject, contactPos, contactNormal);

        Vector2 reflectDirection = Vector2.Reflect(_myTransform.right, contactNormal);
        _myTransform.right = reflectDirection;

        Debug.DrawRay(_myTransform.position, reflectDirection, Color.red, 5);

        _moveComponent.Stop();
        Shoot(reflectDirection);
    }

    public override void OnStateEnter(Vector2 direction, float ratio, string message)
    {
        ShootingTutorialEvent?.Invoke();

        _moveComponent.ApplyMovement = false;
        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Shooting);

        _ratio = ratio;

        _timer.Reset();
        _timer.Start(_playerData.ShootDuration);

        ChangeBodyScale?.Invoke(false, 0);
        Shoot(direction);
        RaycastToRight();
    }

    public override void OnStateExit()
    {
        _moveComponent.ApplyMovement = true;
    }

    void RaycastToRight()
    {
        // direction 방향으로 raycast를 쏴서 앞에 뭔가 있으면 반대로 튕기게 하기
        // 나중에 크기 키우면 변경해줘야함
        float raycastDistance = 1.2f + 0.5f;
        float raycastDistanceFrom = 0f;

        RaycastHit2D hit = Physics2D.Raycast(_myTransform.position + (Vector3)_myTransform.right * raycastDistanceFrom, _myTransform.right, raycastDistance, _layerMask);
        Debug.DrawRay(_myTransform.position + (Vector3)_myTransform.right * raycastDistanceFrom, _myTransform.right * raycastDistance, Color.red, 3);

        //Time.timeScale = 0;

        if (hit.transform != null)
        {
            GameObject hitObject = hit.collider.gameObject;
            Vector3 hitPos = hit.point;
            Vector3 hitNormal = hit.normal;

            ReflectTo(hitObject, hitPos, hitNormal);
        }
    }

    public override void OnStateUpdate()
    {
        if (_timer.CurrentState == Timer.State.Finish)
        {
            GoToReadyState();
            return;
        }
    }

    // 슈팅 캔슬 적용
    public override void OnChargeStart()
    {
        CancelShootingTutorialEvent?.Invoke();
        GoToReadyState();
    }

    void Shoot(Vector2 direction)
    {
        _moveComponent.ResetVelocity();
        _moveComponent.AddForce(direction, _playerData.ShootSpeed * _ratio);
    }

    void GoToReadyState()
    {
        SetInvincible?.Invoke(false);

        ChangeBodyScale?.Invoke(false, 1);
        _baseFSM.SetState(Player.ActionState.Ready);
    }
}
