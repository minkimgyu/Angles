using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TotalCooltimeBuffUpgradeableData
{
    public float _additionalCooltimeRatio;

    public TotalCooltimeBuffUpgradeableData(float additionalCooltimeRatio)
    {
        _additionalCooltimeRatio = additionalCooltimeRatio;
    }
}

public class TotalCooltimeBuffData : BaseBuffData
{
    public List<TotalCooltimeBuffUpgradeableData> _upgradeableDatas;
    public TotalCooltimeBuffData(List<TotalCooltimeBuffUpgradeableData> upgradeableDatas)
    {
        _upgradeableDatas = upgradeableDatas;
    }
}

public class TotalCooltimeBuffCreater : BuffCreater
{
    public TotalCooltimeBuffCreater(BaseBuffData data) : base(data) { }

    public override BaseBuff Create()
    {
        TotalCooltimeBuffData shootingBuffData = _buffData as TotalCooltimeBuffData;
        return new TotalCooltimeBuff(shootingBuffData);
    }
}
