using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbsorbable
{
    void Absorb(Vector3 pos, float force);
}
