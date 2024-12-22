using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class KnockbackUpgrader : BaseSkillUpgrader, IUpgradeVisitor
{
    public struct UpgradableData
    {
        public UpgradableData(float damage, float sizeMultiplier)
        {
            _damage = damage;
            _sizeMultiplier = sizeMultiplier;
        }

        [JsonProperty] private float _damage;
        [JsonProperty] private float _sizeMultiplier;

        [JsonIgnore] public float Damage { get => _damage; }
        [JsonIgnore] public float SizeMultiplier { get => _sizeMultiplier; }
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
        data.RangeMultiplier += upgradeData.SizeMultiplier;

        Debug.Log("_damage: " + data.Damage);
        Debug.Log("_rangeMultiplier: " + data.RangeMultiplier);
    }
}
