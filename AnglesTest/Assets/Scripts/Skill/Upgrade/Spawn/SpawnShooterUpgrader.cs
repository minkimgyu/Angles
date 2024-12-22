using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class SpawnShooterUpgrader : BaseSkillUpgrader, IUpgradeVisitor
{
    public struct UpgradableData
    {
        public UpgradableData(float damage, float delay)
        {
            _damage = damage; // 데미지 비율
            _delay = delay; // 데미지 비율
        }

        public float _damage;
        public float _delay;
    }

    [JsonProperty] List<UpgradableData> _upgradeDatas;

    public SpawnShooterUpgrader(List<UpgradableData> upgradeDatas)
    {
        _upgradeDatas = upgradeDatas;
    }

    public SpawnShooterUpgrader()
    {
    }

    const int _upgradeOffset = 2;

    public void Visit(ISkillUpgradable upgradable, SpawnShooterData data)
    {
        // 2랩부터 업그레이드가 들어가기 때문에 이렇게 한다.
        int index = ReturnUpgradeDataIndex(upgradable.UpgradePoint);
        UpgradableData upgradeData = _upgradeDatas[index];

        data.Damage += upgradeData._damage;
        data.Delay += upgradeData._delay;
    }
}
