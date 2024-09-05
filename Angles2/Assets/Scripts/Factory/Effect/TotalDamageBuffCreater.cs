using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TotalDamageBuffUpgradeableData
{
    public float _additionalDamageRatio;

    public TotalDamageBuffUpgradeableData(float additionalDamageRatio)
    {
        _additionalDamageRatio = additionalDamageRatio;
    }
}

public class TotalDamageBuffData : BaseBuffData
{
    public List<TotalDamageBuffUpgradeableData> _totalDamageBuffUpgradeableDatas;

    public TotalDamageBuffData(List<TotalDamageBuffUpgradeableData> totalDamageBuffUpgradeableDatas)
    {
        _totalDamageBuffUpgradeableDatas = totalDamageBuffUpgradeableDatas;
    }
}

public class TotalDamageBuffCreater : BuffCreater
{
    public TotalDamageBuffCreater(BaseBuffData data) : base(data) { }

    public override BaseBuff Create()
    {
        TotalDamageBuffData shootingBuffData = _buffData as TotalDamageBuffData;
        return new TotalDamageBuff(shootingBuffData);
    }
}
