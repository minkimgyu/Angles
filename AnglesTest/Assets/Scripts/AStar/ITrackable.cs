using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITrackable : IPathfindable
{
    void InjectTarget(ITarget target);
}