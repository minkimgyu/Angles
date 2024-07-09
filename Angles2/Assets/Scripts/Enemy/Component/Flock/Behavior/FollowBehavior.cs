using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBehavior : BaseBehavior
{
    public override Vector3 ReturnVelocity(BehaviorData behaviorData)
    {
        return (behaviorData.PlayerPos - _myTransform.position).normalized * _weight;
    }
}
