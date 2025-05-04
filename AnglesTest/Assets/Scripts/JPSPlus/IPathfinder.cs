using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPathfinder
{
    List<Vector2> FindPath(Vector2 startPos, Vector2 targetPos, BaseEnemy.Size size);
}
