using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPos
{
    Vector3 ReturnPosition();
}

public interface IFlock : IPos
{
    Vector3 ReturnFowardDirection();
}
