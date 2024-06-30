using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignmentBehavior : BaseBehavior
{
    public override Vector3 ReturnVelocity(List<IFlock> nearAgents, List<Transform> nearObstacles)
    {
        if (nearAgents.Count == 0) return transform.forward * _weight;

        Vector3 fowardDir = new Vector3();
        for (int i = 0; i < nearAgents.Count; i++)
        {
            fowardDir += nearAgents[i].ReturnFowardDirection();
        }

        return fowardDir.normalized * _weight;
    }
}
