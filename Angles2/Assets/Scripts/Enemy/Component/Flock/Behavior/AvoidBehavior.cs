using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidBehavior : BaseBehavior
{
    Vector3 RaycastToWall(Vector3 offset)
    {
        int layer = LayerMask.GetMask("Obstacle");

        Vector3 start = _myTransform.position + offset;
        Vector3 dir = _myTransform.right;

        RaycastHit2D hit = Physics2D.Raycast(start, dir, 10, layer);
        if (hit.collider == null) return Vector3.zero;

        Vector3 reflect = Vector2.Reflect(_myTransform.right, hit.normal);

        Debug.DrawRay(start, dir * hit.distance, Color.blue, 3);
        Debug.DrawRay(hit.point, reflect * 5f, Color.red, 3);
        return reflect;
    }

    public override Vector3 ReturnVelocity(BehaviorData behaviorData)
    {
        if (behaviorData.NearObstacles.Count == 0) return Vector3.zero;

        Vector3[] offsets = new Vector3[] {
            new Vector3(0.5f, 0, 0),
            new Vector3(-0.5f, 0, 0),
        };

        Vector3 direction = Vector3.zero;
        for (int i = 0; i < 2; i++) direction += RaycastToWall(offsets[i]);

        return direction.normalized * _weight;
    }
}
