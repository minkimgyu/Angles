using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveUpgrader : IUpgradeVisitor
{
    public struct UpgradableData
    {
        public UpgradableData(float damage, float range)
        {
            _damage = damage;
            _range = range;
        }

        public float _damage;
        public float _range;
    }

    List<UpgradableData> _upgradeDatas;

    public ShockwaveUpgrader(List<UpgradableData> upgradeDatas)
    {
        _upgradeDatas = upgradeDatas;
    }

    public void Visit(ISkillUpgradable upgradable, ShockwaveData data)
    {
        UpgradableData upgradeData = _upgradeDatas[upgradable.UpgradePoint - 1];
        data._damage += upgradeData._damage;
        data._range += upgradeData._range;
    }
}
