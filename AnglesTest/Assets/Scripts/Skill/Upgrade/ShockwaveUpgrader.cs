using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveUpgrader : BaseSkillUpgrader, IUpgradeVisitor
{
    public struct UpgradableData
    {
        public UpgradableData(float damage, float delay, float rangeMultiplier)
        {
            _damage = damage;
            _delay = delay;
            _rangeMultiplier = rangeMultiplier;
        }

        public float _damage;
        public float _delay;
        public float _rangeMultiplier;
    }

    [JsonProperty] List<UpgradableData> _upgradeDatas;

    public ShockwaveUpgrader(List<UpgradableData> upgradeDatas)
    {
        _upgradeDatas = upgradeDatas;
    }

    public ShockwaveUpgrader()
    {
    }

    public void Visit(ISkillUpgradable upgradable, ShockwaveData data)
    {
        int index = GetUpgradeDataIndex(upgradable.UpgradePoint);
        UpgradableData upgradeData = _upgradeDatas[index];

        data.Damage += upgradeData._damage;
        data.Delay += upgradeData._delay;
        data.RangeMultiplier += upgradeData._rangeMultiplier;
    }
}
