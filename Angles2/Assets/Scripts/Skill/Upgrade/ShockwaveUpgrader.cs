using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveUpgrader : BaseSkillUpgrader, IUpgradeVisitor
{
    public struct UpgradableData
    {
        public UpgradableData(float damage, float delay, float sizeMultiplier)
        {
            _damage = damage;
            _delay = delay;
            _sizeMultiplier = sizeMultiplier;
        }

        public float _damage;
        public float _delay;
        public float _sizeMultiplier;
    }

    List<UpgradableData> _upgradeDatas;

    public ShockwaveUpgrader(List<UpgradableData> upgradeDatas)
    {
        _upgradeDatas = upgradeDatas;
    }

    public void Visit(ISkillUpgradable upgradable, ShockwaveData data)
    {
        int index = ReturnUpgradeDataIndex(upgradable.UpgradePoint);
        UpgradableData upgradeData = _upgradeDatas[index];

        data._damage += upgradeData._damage;
        data._delay += upgradeData._delay;
        data._sizeMultiplier += upgradeData._sizeMultiplier;
    }
}
