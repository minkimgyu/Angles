using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour, IObstacle
{
    public Vector3 ReturnPosition()
    {
        return transform.position;
    }
}
