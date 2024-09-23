using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStickyBombUpgrader : IUpgradeVisitor
{
    public struct UpgradableData
    {
        public UpgradableData(float damage, int stackCount)
        {
            _damage = damage;
            _stackCount = stackCount;
        }

        public float _damage;
        public int _stackCount;
    }

    List<UpgradableData> _upgradeDatas;

    public SpawnStickyBombUpgrader(List<UpgradableData> upgradeDatas)
    {
        _upgradeDatas = upgradeDatas;
    }

    public void Visit(ISkillUpgradable upgradable, SpawnStickyBombData data)
    {
        UpgradableData upgradeData = _upgradeDatas[upgradable.UpgradePoint - 1];

        data._damage += upgradeData._damage;
        data._maxStackCount += upgradeData._stackCount;
    }
}
