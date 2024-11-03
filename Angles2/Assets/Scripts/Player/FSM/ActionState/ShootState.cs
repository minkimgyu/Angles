using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShootState : State<Player.ActionState>
{
    Action<bool, float> ChangeBodyScale;

    Action<bool> SetInvincible;
    Action<Collision2D> OnReflect;

    float _ratio;

    Timer _timer;
    Transform _myTransform;
    MoveComponent _moveComponent;

    PlayerData _playerData;

    public ShootState(
        FSM<Player.ActionState> fsm,

        PlayerData playerData,

        Transform myTransform,
        MoveComponent moveComponent,

        Action<bool, float> ChangeBodyScale,
        Action<Collision2D> OnReflect,

        Action<bool> SetInvincible) : base(fsm)
    {
        _playerData = playerData;
        _myTransform = myTransform;
        _moveComponent = moveComponent;

        this.ChangeBodyScale = ChangeBodyScale;
        this.OnReflect = OnReflect;

        _timer = new Timer();
        this.SetInvincible = SetInvincible;
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Bounce, 0.6f);
        OnReflect?.Invoke(collision);

        Vector2 reflectDirection = Vector2.Reflect(_myTransform.right, collision.contacts[0].normal);
        _myTransform.right = reflectDirection;

        Debug.DrawRay(_myTransform.position, reflectDirection, Color.red, 5);

        _moveComponent.Stop();
        Shoot(reflectDirection);
    }

    public override void OnStateEnter(Vector2 direction, float ratio, string message)
    {
        _moveComponent.ApplyMovement = false;
        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Shooting);

        _ratio = ratio;

        _timer.Reset();
        _timer.Start(_playerData._shootDuration);

        ChangeBodyScale?.Invoke(false, 0);
        Shoot(direction);
    }

    public override void OnStateExit()
    {
        _moveComponent.ApplyMovement = true;
    }

    public override void OnStateUpdate()
    {
        if(_timer.CurrentState == Timer.State.Finish)
        {
            GoToReadyState();
            return;
        }
    }

    //// move가 검출된다면 바로 Ready로 보냄
    //public override void OnMove(Vector2 vec2)
    //{
    //    GoToReadyState();
    //}

    public override void OnChargeStart()
    {
        GoToReadyState();
    }

    void Shoot(Vector2 direction)
    {
        _moveComponent.Stop();
        _moveComponent.AddForce(direction, _playerData._shootSpeed * _ratio);
    }

    void GoToReadyState()
    {
        SetInvincible?.Invoke(false);

        ChangeBodyScale?.Invoke(false, 1);
        _baseFSM.SetState(Player.ActionState.Ready);
    }
}
