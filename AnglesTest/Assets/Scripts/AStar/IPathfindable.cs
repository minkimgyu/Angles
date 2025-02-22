using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPathfindable
{
    void InjectPathfindEvent(Func<Vector2, Vector2, BaseLife.Size, List<Vector2>> FindPath);
}
