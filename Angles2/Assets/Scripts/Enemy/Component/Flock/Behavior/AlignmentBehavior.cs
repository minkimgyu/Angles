using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignmentBehavior : BaseBehavior
{
    public override Vector3 ReturnVelocity(BehaviorData behaviorData)
    {
        if (behaviorData.NearAgents.Count == 0) return Vector3.zero;

        Vector3 fowardDir = Vector3.zero;
        for (int i = behaviorData.NearAgents.Count - 1; i >= 0; i--)
        {
            if ((behaviorData.NearAgents[i] as UnityEngine.Object) == null) continue;
            fowardDir += behaviorData.NearAgents[i].ReturnFowardDirection();
        }

        return fowardDir.normalized * _weight;
    }
}
