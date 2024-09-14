using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBladeUpgrader : IUpgradeVisitor
{
    public struct UpgradableData
    {
        public UpgradableData(float damage, float attackDelay, float range)
        {
            _damage = damage;
            _attackDelay = attackDelay;
            _range = range;
        }

        public float _damage;
        public float _attackDelay;
        public float _range;
    }

    List<UpgradableData> _upgradeDatas;

    public SpawnBladeUpgrader(List<UpgradableData> upgradeDatas)
    {
        _upgradeDatas = upgradeDatas;
    }

    public void Visit(IUpgradable upgradable, BladeData data)
    {
        UpgradableData upgradeData = _upgradeDatas[upgradable.UpgradePoint - 1];
        data += upgradeData;
    }
}
