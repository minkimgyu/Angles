using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IForce
{
    bool CanAbsorb();

    void ApplyForce(Vector3 pos, float force, ForceMode2D mode);
}
