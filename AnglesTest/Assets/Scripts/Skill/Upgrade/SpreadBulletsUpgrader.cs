using DamageUtility;
using Newtonsoft.Json;
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

    [JsonProperty] List<UpgradableData> _upgradeDatas;

    public SpreadBulletsUpgrader(List<UpgradableData> datas)
    {
        _upgradeDatas = datas;
    }

    public SpreadBulletsUpgrader()
    {
    }

    public void Visit(ISkillUpgradable upgradable, SpreadBulletsData data) 
    {
        int index = ReturnUpgradeDataIndex(upgradable.UpgradePoint);
        UpgradableData upgradeData = _upgradeDatas[index];

        data.Delay += upgradeData._delay;
        data.Damage += upgradeData._damage;
        data.Force += upgradeData._force;
    }
}
