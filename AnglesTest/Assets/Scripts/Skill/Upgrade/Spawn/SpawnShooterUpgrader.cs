using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnShooterUpgrader : BaseSkillUpgrader, IUpgradeVisitor
{
    public struct UpgradableData
    {
        public UpgradableData(float damage, float delay)
        {
            _damage = damage; // ������ ����
            _delay = delay; // ������ ����
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
        // 2������ ���׷��̵尡 ���� ������ �̷��� �Ѵ�.
        int index = ReturnUpgradeDataIndex(upgradable.UpgradePoint);
        UpgradableData upgradeData = _upgradeDatas[index];

        data._damage += upgradeData._damage;
        data._delay += upgradeData._delay;
    }
}
