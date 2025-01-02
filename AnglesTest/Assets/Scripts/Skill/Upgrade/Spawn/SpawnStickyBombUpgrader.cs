using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

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

    [JsonProperty] List<UpgradableData> _upgradeDatas;

    public SpawnStickyBombUpgrader(List<UpgradableData> upgradeDatas)
    {
        _upgradeDatas = upgradeDatas;
    }

    public SpawnStickyBombUpgrader()
    {
    }

    public void Visit(ISkillUpgradable upgradable, SpawnStickyBombData data)
    {
        // 2������ ���׷��̵尡 ���� ������ �̷��� �Ѵ�.
        int index = ReturnUpgradeDataIndex(upgradable.UpgradePoint);
        UpgradableData upgradeData = _upgradeDatas[index];

        data.Damage += upgradeData._damage;
        data.MaxStackCount += upgradeData._stackCount;
    }
}
