using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingStatModifier : IStatModifier
{
    public struct ShootingStat
    {
        public float _additionalDuration; // 날라가는 시간 증가
        public float _additionalChargeDuration; // 차지 시간 감소

        public ShootingStat(float additionalDuration, float additionalChargeDuration)
        {
            _additionalDuration = additionalDuration;
            _additionalChargeDuration = additionalChargeDuration;
        }
    }

    List<ShootingStat> _shootingStats;
    ShootingStat _shootingStat;

    public ShootingStatModifier(List<ShootingStat> shootingStats)
    {
        _shootingStats = shootingStats;
    }

    public ShootingStatModifier(ShootingStat shootingStat)
    {
        _shootingStat = shootingStat;
    }

    public void Visit<T>(T data, int level) where T : PlayerData
    {
        data._shootDuration += _shootingStats[level]._additionalDuration;
        data._chargeDuration += _shootingStats[level]._additionalChargeDuration;
    }

    public void Visit<T>(T data) where T : PlayerData
    {
        data._shootDuration += _shootingStat._additionalDuration;
        data._chargeDuration += _shootingStat._additionalChargeDuration;
    }
}