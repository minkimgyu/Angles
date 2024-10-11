using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStickyBombUpgrader : BaseSkillUpgrader, IUpgradeVisitor
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
        // 2랩부터 업그레이드가 들어가기 때문에 이렇게 한다.
        int index = ReturnUpgradeDataIndex(upgradable.UpgradePoint);
        UpgradableData upgradeData = _upgradeDatas[index];

        data._damage += upgradeData._damage;
        data._maxStackCount += upgradeData._stackCount;
    }
}
