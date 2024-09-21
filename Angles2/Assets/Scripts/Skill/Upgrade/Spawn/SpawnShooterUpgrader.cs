using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnShooterUpgrader : IUpgradeVisitor
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

    List<UpgradableData> _upgradeDatas;

    public SpawnShooterUpgrader(List<UpgradableData> upgradeDatas)
    {
        _upgradeDatas = upgradeDatas;
    }

    public void Visit(ISkillUpgradable upgradable, SpawnShooterData data)
    {
        UpgradableData upgradeData = _upgradeDatas[upgradable.UpgradePoint - 1];

        data._damage += upgradeData._damage;
        data._delay += upgradeData._delay;
    }
}
