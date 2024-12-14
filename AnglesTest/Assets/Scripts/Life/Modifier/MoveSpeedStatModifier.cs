using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpeedStatModifier : IStatModifier
{
    List<float> _additionalMoveSpeeds;
    float _additionalMoveSpeed;

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
        data._moveSpeed += _additionalMoveSpeeds[level];
    }

    public void Visit<T>(T data) where T : PlayerData
    {
        data._moveSpeed += _additionalMoveSpeed;
    }
}
