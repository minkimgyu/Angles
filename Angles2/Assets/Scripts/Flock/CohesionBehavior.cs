using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CohesionBehavior : BaseBehavior
{
    public override Vector3 ReturnVelocity(List<IFlock> nearAgents, List<Transform> nearObstacles)
    {
        if (nearAgents.Count == 0) return Vector3.zero;

        Vector3 combinedPos = new Vector3();
        for (int i = 0; i < nearAgents.Count; i++)
        {
            combinedPos += nearAgents[i].ReturnPosition();
        }

        combinedPos /= nearAgents.Count;
        return (combinedPos - transform.position).normalized * _weight;
    }
}
