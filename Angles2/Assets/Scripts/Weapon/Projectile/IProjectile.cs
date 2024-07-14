using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{
    Vector2 ReturnDirectionVector();

    void Shoot(Vector3 direction, float force);
}
