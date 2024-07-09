using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockComponent : MonoBehaviour
{
    public enum BehaviorType
    {
        Alignment,
        Cohesion,
        Separation,
        Stay,
        Avoid,
        Follow
    }

    Vector3 _direction;

    List<BaseBehavior> _behaviors;
    [SerializeField] FlockBehaviorDictionary _behaviorDictionary;

    BaseBehavior ReturnBehavior(BehaviorType type)
    {
        switch (type)
        {
            case BehaviorType.Alignment:
                return new AlignmentBehavior();
            case BehaviorType.Cohesion:
                return new CohesionBehavior();
            case BehaviorType.Separation:
                return new SeparationBehavior();
            case BehaviorType.Stay:
                return new StayInRadiusBehavior();
            case BehaviorType.Avoid:
                return new AvoidBehavior();
            case BehaviorType.Follow:
                return new FollowBehavior();
            default:
                break;
        }

        return null;
    }

    public void Initialize()
    {
        _direction = Vector2.zero;
        _behaviors = new List<BaseBehavior>();

        foreach (var item in _behaviorDictionary)
        {
            BaseBehavior behavior = ReturnBehavior(item.Key);
            behavior.Intialize(transform, item.Value);

            _behaviors.Add(behavior);
        }
    }

    public Vector3 ReturnDirection(BehaviorData behaviorData)
    {
        Vector3 newDirection = Vector3.zero;
        for (int i = 0; i < _behaviors.Count; i++)
        {
            newDirection += _behaviors[i].ReturnVelocity(behaviorData);
        }

        _direction = Vector3.Lerp(_direction, newDirection, Time.deltaTime);
        return _direction;
    }
}
