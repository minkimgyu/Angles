using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackUpgrader : IUpgradeVisitor
{
    public struct UpgradableData
    {
        public UpgradableData(float damage, float sizeMultiplier)
        {
            _damage = damage;
            _sizeMultiplier = sizeMultiplier;
        }

        public float _damage;
        public float _sizeMultiplier;
    }

    List<UpgradableData> _upgradeDatas;

    public KnockbackUpgrader(List<UpgradableData> upgradeDatas)
    {
        _upgradeDatas = upgradeDatas;
    }

    public void Visit(ISkillUpgradable upgradable, KnockbackData data)
    {
        UpgradableData upgradeData = _upgradeDatas[upgradable.UpgradePoint - 1];
        data._damage += upgradeData._damage;
        data._rangeMultiplier += upgradeData._sizeMultiplier;

        Debug.Log("_damage: " + data._damage);
        Debug.Log("_rangeMultiplier: " + data._rangeMultiplier);
    }
}
