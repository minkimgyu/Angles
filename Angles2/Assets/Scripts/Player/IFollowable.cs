using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFollowable : IPos
{
    bool CanFollow();

    Vector3 ReturnFowardDirection();
}
