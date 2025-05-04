using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindInjecter : MonoBehaviour
{
    IPathfinder _pathfinder;

    public void Initialize(IPathfinder pathfinder)
    {
        _pathfinder = pathfinder;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IPathfindable pathfindable = collision.GetComponentInParent<IPathfindable>();
        if (pathfindable == null) return;

        pathfindable.InjectPathfindEvent(_pathfinder.FindPath);
    }
}
