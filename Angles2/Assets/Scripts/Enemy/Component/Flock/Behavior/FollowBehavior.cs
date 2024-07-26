using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBehavior : BaseBehavior
{
    public override Vector3 ReturnVelocity(BehaviorData behaviorData)
    {
        Vector3 velocity;

        Vector3 direction = behaviorData.TargetPos - _myTransform.position;
        velocity = direction.normalized * _weight;
        return velocity;
    }
}
