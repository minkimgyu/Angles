using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class ImpactUpgrader : BaseSkillUpgrader, IUpgradeVisitor
{
    public struct UpgradableData
    {
        public UpgradableData(float damage, float rangeMultiplier)
        {
            _damage = damage;
            _rangeMultiplier = rangeMultiplier;
        }

        public float _damage;
        public float _rangeMultiplier;
    }

    [JsonProperty] List<UpgradableData> _upgradeDatas;

    public ImpactUpgrader(List<UpgradableData> upgradeDatas)
    {
        _upgradeDatas = upgradeDatas;
    }

    public ImpactUpgrader()
    {
    }

    public void Visit(ISkillUpgradable upgradable, ImpactData data)
    {
        // 2랩부터 업그레이드가 들어가기 때문에 이렇게 한다.
        int index = GetUpgradeDataIndex(upgradable.UpgradePoint);
        UpgradableData upgradeData = _upgradeDatas[index];

        data.Damage += upgradeData._damage;
        data.RangeMultiplier += upgradeData._rangeMultiplier;

        Debug.Log("_damage: " + data.Damage);
        Debug.Log("_rangeMultiplier: " + data.RangeMultiplier);
    }
}
