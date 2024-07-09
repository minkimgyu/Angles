using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Player.FSM
{
    public class ReflectState : State
    {
        Action<Vector2, string> RevertToPreviousState;
        Transform _myTransform;

        public ReflectState(Transform myTransform, Action<Vector2, string> RevertToPreviousState)
        {
            _myTransform = myTransform;
            this.RevertToPreviousState = RevertToPreviousState;
        }

        public override void OnStateEnter(Vector2 normal, string message)
        {
            Vector2 reflectDirection = Vector2.Reflect(_myTransform.right, normal);
            _myTransform.right = reflectDirection;

            RevertToPreviousState(reflectDirection, "RevertToShootState");
        }
    }
}