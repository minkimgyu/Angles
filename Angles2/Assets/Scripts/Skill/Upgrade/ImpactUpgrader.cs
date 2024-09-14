using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactUpgrader : IUpgradeVisitor
{
    public struct UpgradableData
    {
        public UpgradableData(float damage, float range)
        {
            _damage = damage;
            _range = range;
        }

        private float _damage;
        private float _range;

        public float Damage { get => _damage; }
        public float Range { get => _range; }
    }

    List<UpgradableData> _upgradeDatas;

    public ImpactUpgrader(List<UpgradableData> upgradeDatas)
    {
        _upgradeDatas = upgradeDatas;
    }

    public void Visit(IUpgradable upgradable, ImpactData data)
    {
        UpgradableData upgradeData = _upgradeDatas[upgradable.UpgradePoint - 1];
        data += upgradeData;
    }
}
