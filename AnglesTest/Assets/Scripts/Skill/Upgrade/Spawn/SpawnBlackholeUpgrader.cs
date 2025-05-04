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
        // 2랩부터 업그레이드가 들어가기 때문에 이렇게 한다.
        int index = GetUpgradeDataIndex(upgradable.UpgradePoint);
        UpgradableData upgradeData = _upgradeDatas[index];

        data.Lifetime += upgradeData._lifeTime;
        data.SizeMultiplier += upgradeData._range;
    }
}
