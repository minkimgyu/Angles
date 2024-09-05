using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DashBuffUpgradeableData
{
    public float _additionalSpeed;
    public float _additionalChargeDuration;

    public DashBuffUpgradeableData(float additionalSpeed, float additionalChargeDuration)
    {
        _additionalSpeed = additionalSpeed;
        _additionalChargeDuration = additionalChargeDuration;
    }
}

public class DashBuffData : BaseBuffData
{
    public List<DashBuffUpgradeableData> _upgradeableDatas;

    public DashBuffData(List<DashBuffUpgradeableData> upgradeableDatas)
    {
        _upgradeableDatas = upgradeableDatas;
    }
}

public class DashBuffCreater : BuffCreater
{
    public DashBuffCreater(BaseBuffData data) : base(data) { }

    public override BaseBuff Create()
    {
        DashBuffData shootingBuffData = _buffData as DashBuffData;
        return new DashBuff(shootingBuffData);
    }
}

