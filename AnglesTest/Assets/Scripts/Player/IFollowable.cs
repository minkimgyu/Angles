using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFollowable : IPos
{
    Vector2 BottomPoint { get; }

    bool CanFollow();
    Vector3 ReturnFowardDirection();
}