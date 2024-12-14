using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructionUpgrader : BaseSkillUpgrader, IUpgradeVisitor
{
    public struct UpgradableData
    {
        public UpgradableData(float damage, float range, int delay)
        {
            _delay = delay;
            _damage = damage;
            _range = range;
        }

        public int _delay;
        public float _damage;
        public float _range;
    }

    List<UpgradableData> _upgradeDatas;

    public SelfDestructionUpgrader(List<UpgradableData> upgradeDatas)
    {
        _upgradeDatas = upgradeDatas;
    }

    public void Visit(ISkillUpgradable upgradable, SelfDestructionData data)
    {
        int index = ReturnUpgradeDataIndex(upgradable.UpgradePoint);
        UpgradableData upgradeData = _upgradeDatas[index];

        data._damage += upgradeData._damage;
        data._range += upgradeData._range;
        data._delay += upgradeData._delay;
    }
}
