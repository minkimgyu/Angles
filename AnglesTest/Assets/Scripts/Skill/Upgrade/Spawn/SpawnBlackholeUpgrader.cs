using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
public class SpawnBlackholeUpgrader : BaseSkillUpgrader, IUpgradeVisitor
{
    public struct UpgradableData
    {
        public UpgradableData(float lifeTime, float range)
        {
            _lifeTime = lifeTime;
            _range = range;
        }

        public float _lifeTime;
        public float _range;
    }

    [JsonProperty] List<UpgradableData> _upgradeDatas;

    public SpawnBlackholeUpgrader(List<UpgradableData> upgradeDatas)
    {
        _upgradeDatas = upgradeDatas;
    }

    public SpawnBlackholeUpgrader()
    {
    }

    public void Visit(ISkillUpgradable upgradable, SpawnBlackholeData data) 
    {
        // 2������ ���׷��̵尡 ���� ������ �̷��� �Ѵ�.
        int index = GetUpgradeDataIndex(upgradable.UpgradePoint);
        UpgradableData upgradeData = _upgradeDatas[index];

        data.Lifetime += upgradeData._lifeTime;
        data.SizeMultiplier += upgradeData._range;
    }
}
