using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatUpgrader : IUpgradeVisitor
{
    public struct DamageData
    {
        public float _additionalDamageRatio;

        public DamageData(float additionalDamageRatio)
        {
            _additionalDamageRatio = additionalDamageRatio;
        }
    }

    public struct CooltimeData
    {
        public float _additionalCooltimeRatio;

        public CooltimeData(float additionalCooltimeRatio)
        {
            _additionalCooltimeRatio = additionalCooltimeRatio;
        }
    }

    public struct ShootingData
    {
        public float _additionalDuration; // 날라가는 시간 증가
        public float _additionalChargeDuration; // 차지 시간 감소

        public ShootingData(float additionalDuration, float additionalChargeDuration)
        {
            _additionalDuration = additionalDuration;
            _additionalChargeDuration = additionalChargeDuration;
        }
    }

    public struct DashData
    {
        public float _additionalSpeed;
        public float _additionalRestoreDuration;

        public DashData(float additionalSpeed, float additionalRestoreDuration)
        {
            _additionalSpeed = additionalSpeed;
            _additionalRestoreDuration = additionalRestoreDuration;
        }
    }

    public void Visit(PlayerData data, DashData dashData)
    {
        data._dashSpeed += dashData._additionalSpeed;
        data._dashRestoreDuration += dashData._additionalRestoreDuration;
    }

    public void Visit(PlayerData data, ShootingData shootingData) 
    {
        data._shootDuration += shootingData._additionalDuration;
        data._chargeDuration += shootingData._additionalChargeDuration;
    }

    public void Visit(PlayerData data, CooltimeData cooltimeData)
    {
        data.TotalCooltimeRatio += cooltimeData._additionalCooltimeRatio;
    }

    public void Visit(PlayerData data, DamageData damageData)
    {
        data.TotalDamageRatio += damageData._additionalDamageRatio;
    }
}
