using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayInRadiusBehavior : BaseBehavior
{
    public Vector3 center = Vector3.zero;
    public float radius = 18f;

    public override Vector3 ReturnVelocity(BehaviorData behaviorData)
    {
        Vector3 centerOffset = center - _myTransform.position;
        float t = centerOffset.magnitude / radius;
        if (t < 0.9f)
        {
            return Vector3.zero;
        }

        return centerOffset * t * t;
    }
}
