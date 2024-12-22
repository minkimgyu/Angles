using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MoveSpeedStatModifier : IStatModifier
{
    [JsonProperty] List<float> _additionalMoveSpeeds;
    [JsonProperty] float _additionalMoveSpeed;

    public MoveSpeedStatModifier() { }

    public MoveSpeedStatModifier(List<float> additionalMoveSpeeds)
    {
        _additionalMoveSpeeds = additionalMoveSpeeds;
    }

    public MoveSpeedStatModifier(float additionalMoveSpeed)
    {
        _additionalMoveSpeed = additionalMoveSpeed;
    }

    public void Visit<T>(T data, int level) where T : PlayerData
    {
        data.MoveSpeed += _additionalMoveSpeeds[level];
    }

    public void Visit<T>(T data) where T : PlayerData
    {
        data.MoveSpeed += _additionalMoveSpeed;
    }
}
