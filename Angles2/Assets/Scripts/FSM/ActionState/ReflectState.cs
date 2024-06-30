using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ReflectState : State
{
    Action<Vector2, string> RevertToPreviousState;
    Transform myTransform;

    public ReflectState(Transform myTransform, Action<Vector2, string> RevertToPreviousState)
    {
        this.myTransform = myTransform;

        this.RevertToPreviousState = RevertToPreviousState;
    }

    Vector2 ReturnReflectDirection(Vector2 point)
    {
        return (Vector2)myTransform.position - point;
    }

    public override void OnStateEnter(Vector2 point, string message)
    {
        Vector2 reflectDirection = ReturnReflectDirection(point);
        RevertToPreviousState(reflectDirection, "RevertToShootState");
    }
}
