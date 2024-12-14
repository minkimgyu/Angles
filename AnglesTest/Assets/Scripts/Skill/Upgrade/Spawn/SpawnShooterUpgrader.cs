using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    List<UpgradableData> _upgradeDatas;

    public SpawnShooterUpgrader(List<UpgradableData> upgradeDatas)
    {
        _upgradeDatas = upgradeDatas;
    }

    const int _upgradeOffset = 2;

    public void Visit(ISkillUpgradable upgradable, SpawnShooterData data)
    {
        // 2랩부터 업그레이드가 들어가기 때문에 이렇게 한다.
        int index = ReturnUpgradeDataIndex(upgradable.UpgradePoint);
        UpgradableData upgradeData = _upgradeDatas[index];

        data._damage += upgradeData._damage;
        data._delay += upgradeData._delay;
    }
}
