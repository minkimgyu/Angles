using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ShootingBuffUpgradeable
{
    public float _additionalDuration;
    public float _additionalChargeDuration;

    public ShootingBuffUpgradeable(float additionalDuration, float additionalChargeDuration)
    {
        _additionalDuration = additionalDuration;
        _additionalChargeDuration = additionalChargeDuration;
    }
}

public class ShootingBuffData : BaseBuffData
{
    public List<ShootingBuffUpgradeable> _shootingBuffUpgradeableDatas;

    public ShootingBuffData(List<ShootingBuffUpgradeable> shootingBuffUpgradeableDatas)
    {
        _shootingBuffUpgradeableDatas = shootingBuffUpgradeableDatas;
    }
}

public class ShootingBuffCreater : BuffCreater
{
    public ShootingBuffCreater(BaseBuffData data) : base(data) { }

    public override BaseBuff Create()
    {
        ShootingBuffData shootingBuffData = _buffData as ShootingBuffData;
        return new ShootingBuff(shootingBuffData);
    }
}
