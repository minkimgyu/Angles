using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class KnockbackUpgrader : BaseSkillUpgrader, IUpgradeVisitor
{
    public struct UpgradableData
    {
        public UpgradableData(float damage, float rangeMultiplier)
        {
            _damage = damage;
            _rangeMultiplier = rangeMultiplier;
        }

        [JsonProperty] private float _damage;
        [JsonProperty] private float _rangeMultiplier;

        [JsonIgnore] public float Damage { get => _damage; }
        [JsonIgnore] public float RangeMultiplier { get => _rangeMultiplier; }
    }

    [JsonProperty] List<UpgradableData> _upgradeDatas;

    public KnockbackUpgrader(List<UpgradableData> upgradeDatas)
    {
        _upgradeDatas = upgradeDatas;
    }

    public KnockbackUpgrader()
    {
    }

    const int _upgradeOffset = 2;

    public void Visit(ISkillUpgradable upgradable, KnockbackData data)
    {
        // 2랩부터 업그레이드가 들어가기 때문에 이렇게 한다.
        int index = ReturnUpgradeDataIndex(upgradable.UpgradePoint);
        UpgradableData upgradeData = _upgradeDatas[index];

        data.Damage += upgradeData.Damage;
        data.RangeMultiplier += upgradeData.RangeMultiplier;

        Debug.Log("_damage: " + data.Damage);
        Debug.Log("_rangeMultiplier: " + data.RangeMultiplier);
    }
}
