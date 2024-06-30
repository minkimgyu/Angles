using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseState
{
    public abstract void OnStateEnter();
    public abstract void OnStateEnter(Vector2 vec2, string message);

    public abstract void OnStateUpdate();
    public abstract void OnStateExit();

    public abstract void OnCollisionEnter(Collision2D collision);

    public abstract void OnMoveStart();
    public abstract void OnMove(Vector2 vec2);
    public abstract void OnMoveEnd();

    public abstract void OnChargeStart();
    public abstract void OnCharge(Vector2 vec2);
    public abstract void OnChargeEnd();

    public abstract void OnDash(); 
}

public class State : BaseState
{
    public override void OnStateEnter() { }
    public override void OnStateEnter(Vector2 vec2, string message) { }

    public override void OnStateExit() { }
    public override void OnStateUpdate() { }

    public override void OnCollisionEnter(Collision2D collision) { }

    public override void OnMoveStart() { }
    public override void OnMove(Vector2 vec2) { }
    public override void OnMoveEnd() { }

    public override void OnChargeStart() { }
    public override void OnCharge(Vector2 vec2) { }
    public override void OnChargeEnd() { }

    public override void OnDash() { }
}
