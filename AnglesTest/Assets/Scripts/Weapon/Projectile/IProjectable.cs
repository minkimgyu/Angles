using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IProjectable
{
    void Shoot(Vector3 direction, float force);
}

public interface ITrackable
{
    void InjectTarget(ITarget target);
}