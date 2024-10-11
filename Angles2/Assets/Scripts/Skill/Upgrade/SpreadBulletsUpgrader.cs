using DamageUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadBulletsUpgrader : BaseSkillUpgrader, IUpgradeVisitor
{
    public struct UpgradableData
    {
        public UpgradableData(float delay, float damage, float force)
        {
            _delay = delay;
            _damage = damage;
            _force = force;
        }

        public float _delay;
        public float _damage;
        public float _force;
    }

    List<UpgradableData> _upgradeDatas;

    public SpreadBulletsUpgrader(List<UpgradableData> datas)
    {
        _upgradeDatas = datas;
    }

    public void Visit(ISkillUpgradable upgradable, SpreadBulletsData data) 
    {
        int index = ReturnUpgradeDataIndex(upgradable.UpgradePoint);
        UpgradableData upgradeData = _upgradeDatas[index];

        data._delay += upgradeData._delay;
        data._damage += upgradeData._damage;
        data._force += upgradeData._force;
    }
}
