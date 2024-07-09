using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignmentBehavior : BaseBehavior
{
    public override Vector3 ReturnVelocity(BehaviorData behaviorData)
    {
        if (behaviorData.NearAgents.Count == 0) return _myTransform.up * _weight;

        Vector3 fowardDir = Vector3.zero;
        for (int i = 0; i < behaviorData.NearAgents.Count; i++)
        {
            fowardDir += behaviorData.NearAgents[i].ReturnFowardDirection();
        }

        return fowardDir.normalized * _weight;
    }
}
