using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticFieldUpgrader : BaseSkillUpgrader, IUpgradeVisitor
{
    public struct UpgradableData
    {
        public UpgradableData(float damage, float delay)
        {
            _damage = damage;
            _delay = delay;
        }

        public float _damage;
        public float _delay;
    }

    List<UpgradableData> _upgradeDatas;

    public MagneticFieldUpgrader(List<UpgradableData> upgradeDatas)
    {
        _upgradeDatas = upgradeDatas;
    }

    public void Visit(ISkillUpgradable upgradable, MagneticFieldData data)
    {
        int index = ReturnUpgradeDataIndex(upgradable.UpgradePoint);
        UpgradableData upgradeData = _upgradeDatas[index];

        data._damage += upgradeData._damage;
        data._delay += upgradeData._delay;
    }
}
