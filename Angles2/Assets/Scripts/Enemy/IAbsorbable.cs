using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbsorbable
{
    bool CanAbsorb();

    void Absorb(Vector3 pos, float force);
}
