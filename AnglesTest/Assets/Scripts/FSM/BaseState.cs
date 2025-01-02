using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseState<T>
{
    protected BaseFSM<T> _baseFSM;
    public BaseState(FSM<T> fsm) { _baseFSM = fsm; }

    public abstract void OnStateEnter();
    public abstract void OnStateEnter(ITarget target, string message);
    public abstract void OnStateEnter(Vector2 vec2, string message);
    public abstract void OnStateEnter(Vector2 vec2, float ratio, string message);
    public abstract void OnStateEnter(Collision2D collision, string message);

    public abstract void OnStateUpdate();
    public abstract void OnFixedUpdate();

    public abstract void OnStateExit();

    public abstract void OnCollisionEnter(Collision2D collision);

    public abstract void OnMoveStart();
    public abstract void OnMove(Vector2 vec2);
    public abstract void OnMoveEnd();

    public abstract void OnChargeStart();
    public abstract void OnCharge(Vector2 vec2);
    public abstract void OnChargeEnd();

    public abstract void OnDash();

    public abstract void OnTargetEnter(ITarget target);
    public abstract void OnTargetExit(ITarget target);
}

public class State<T> : BaseState<T>
{
    public State(FSM<T> baseFSM) : base(baseFSM) {}

    public override void OnStateEnter() { }
    public override void OnStateEnter(ITarget target, string message) { }
    public override void OnStateEnter(Vector2 vec2, string message) { }
    public override void OnStateEnter(Vector2 vec2, float ratio, string message) { }
    public override void OnStateEnter(Collision2D collision, string message) { }


    public override void OnStateExit() { }


    public override void OnStateUpdate() { }
    public override void OnFixedUpdate() { }

    public override void OnCollisionEnter(Collision2D collision) { }

    public override void OnMoveStart() { }
    public override void OnMove(Vector2 vec2) { }
    public override void OnMoveEnd() { }

    public override void OnChargeStart() { }
    public override void OnCharge(Vector2 vec2) { }
    public override void OnChargeEnd() { }

    public override void OnDash() { }

    public override void OnTargetEnter(ITarget target) { }
    public override void OnTargetExit(ITarget target) { }
}
