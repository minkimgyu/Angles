using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBladeUpgrader : BaseSkillUpgrader, IUpgradeVisitor
{
    public struct UpgradableData
    {
        public UpgradableData(float damage, float lifetime, float range)
        {
            _damage = damage;
            _lifetime = lifetime;
            _range = range;
        }

        public float _damage;
        public float _lifetime;
        public float _range;
    }

    [JsonProperty] List<UpgradableData> _upgradeDatas;

    public SpawnBladeUpgrader(List<UpgradableData> upgradeDatas)
    {
        _upgradeDatas = upgradeDatas;
    }

    public SpawnBladeUpgrader()
    {
    }

    const int _upgradeOffset = 2;

    public void Visit(ISkillUpgradable upgradable, SpawnBladeData data)
    {
        // 2랩부터 업그레이드가 들어가기 때문에 이렇게 한다.
        int index = GetUpgradeDataIndex(upgradable.UpgradePoint);
        UpgradableData upgradeData = _upgradeDatas[index];

        data.Damage += upgradeData._damage;
        data.SizeMultiplier += upgradeData._range;
        data.Lifetime += upgradeData._lifetime;
    }
}
