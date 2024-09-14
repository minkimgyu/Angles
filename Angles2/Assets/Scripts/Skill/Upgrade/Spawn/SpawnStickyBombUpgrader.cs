using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStickyBombUpgrader : IUpgradeVisitor
{
    public struct UpgradableData
    {
        public UpgradableData(float damage, float range, float explosionDelay)
        {
            _damage = damage;
            _range = range;
            _explosionDelay = explosionDelay;
        }

        public float _damage;
        public float _range;
        public float _explosionDelay;
    }

    List<UpgradableData> _upgradeDatas;

    public SpawnStickyBombUpgrader(List<UpgradableData> upgradeDatas)
    {
        _upgradeDatas = upgradeDatas;
    }

    public void Visit(IUpgradable upgradable, StickyBombData data)
    {
        UpgradableData upgradeData = _upgradeDatas[upgradable.UpgradePoint - 1];
        data += upgradeData;
    }
}
