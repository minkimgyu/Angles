using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoHpRecoveryStatModifier : IStatModifier
{
    List<float> _additionalAutoHpRecoveryPoints;
    float _additionalAutoHpRecoveryPoint;

    public AutoHpRecoveryStatModifier(List<float> additionalAutoHpRecoveryPoints)
    {
        _additionalAutoHpRecoveryPoints = additionalAutoHpRecoveryPoints;
    }

    public AutoHpRecoveryStatModifier(float additionalAutoHpRecoveryPoint)
    {
        _additionalAutoHpRecoveryPoint = additionalAutoHpRecoveryPoint;
    }

    public void Visit<T>(T data, int level) where T : PlayerData
    {
        data.AutoHpRecoveryPoint += _additionalAutoHpRecoveryPoints[level];
    }

    public void Visit<T>(T data) where T : PlayerData
    {
        data.AutoHpRecoveryPoint += _additionalAutoHpRecoveryPoint;
    }
}
