using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeparationBehavior : BaseBehavior
{
    public override Vector3 ReturnVelocity(BehaviorData behaviorData)
    {
        if (behaviorData.NearAgents.Count == 0) return Vector3.zero;

        Vector3 combinedPos = new Vector3();
        for (int i = 0; i < behaviorData.NearAgents.Count; i++)
        {
            combinedPos += behaviorData.NearAgents[i].ReturnPosition();
        }

        combinedPos /= behaviorData.NearAgents.Count;
        return (_myTransform.position - combinedPos).normalized * _weight;
    }
}
