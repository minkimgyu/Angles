using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ReflectState : State<Player.ActionState>
{
    Transform _myTransform;

    public ReflectState(FSM<Player.ActionState> fsm, Transform myTransform)
    : base(fsm)
    {
        _myTransform = myTransform;
    }

    public override void OnStateEnter(Vector2 normal, string message)
    {
        Vector2 reflectDirection = Vector2.Reflect(_myTransform.right, normal);
        _myTransform.right = reflectDirection;
        _baseFSM.RevertToPreviousState(reflectDirection, "RevertToShootState");
    }
}