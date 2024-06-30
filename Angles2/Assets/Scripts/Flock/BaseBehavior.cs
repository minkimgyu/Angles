using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseBehavior : MonoBehaviour
{
    [SerializeField] protected float _weight = 1;
    public virtual void Intialize(Transform target) { }
    public abstract Vector3 ReturnVelocity(List<IFlock> nearAgents, List<Transform> nearObstacles);
}
