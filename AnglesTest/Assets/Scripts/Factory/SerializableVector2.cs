using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SerializableVector2
{
    public SerializableVector2(Vector2 vector)
    {
        this.x = vector.x;
        this.y = vector.y;
    }

    public SerializableVector2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    [JsonIgnore]
    public Vector2 V2 { get {  return new Vector2(x, y); } }

    public float x;
    public float y;
}