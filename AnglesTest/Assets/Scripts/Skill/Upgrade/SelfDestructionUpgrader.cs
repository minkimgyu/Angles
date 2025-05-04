using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructionUpgrader : BaseSkillUpgrader, IUpgradeVisitor
{
    public struct UpgradableData
    {
        public UpgradableData(float damage, float rangeMultiplier, int delay)
        {
            _delay = delay;
            _damage = damage;
            _rangeMultiplier = rangeMultiplier;
        }

        public int _delay;
        public float _damage;
        public float _rangeMultiplier;
    }

    [JsonProperty] List<UpgradableData> _upgradeDatas;

    public SelfDestructionUpgrader(List<UpgradableData> upgradeDatas)
    {
        _upgradeDatas = upgradeDatas;
    }

    public SelfDestructionUpgrader()
    {
    }

    public void Visit(ISkillUpgradable upgradable, SelfDestructionData data)
    {
        int index = GetUpgradeDataIndex(upgradable.UpgradePoint);
        UpgradableData upgradeData = _upgradeDatas[index];

        data.Damage += upgradeData._damage;
        data.RangeMultiplier += upgradeData._rangeMultiplier;
        data.Delay += upgradeData._delay;
    }
}
