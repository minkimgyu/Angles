using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SerializableVector2
{
    public SerializableVector2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public float x;
    public float y;
}