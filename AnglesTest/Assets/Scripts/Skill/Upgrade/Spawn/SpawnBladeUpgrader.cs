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

    List<UpgradableData> _upgradeDatas;

    public SpawnBladeUpgrader(List<UpgradableData> upgradeDatas)
    {
        _upgradeDatas = upgradeDatas;
    }

    const int _upgradeOffset = 2;

    public void Visit(ISkillUpgradable upgradable, SpawnBladeData data)
    {
        // 2������ ���׷��̵尡 ���� ������ �̷��� �Ѵ�.
        int index = ReturnUpgradeDataIndex(upgradable.UpgradePoint);
        UpgradableData upgradeData = _upgradeDatas[index];

        data._damage += upgradeData._damage;
        data._sizeMultiplier += upgradeData._range;
        data._lifetime += upgradeData._lifetime;
    }
}