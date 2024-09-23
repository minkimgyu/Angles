using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlackholeUpgrader : IUpgradeVisitor
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

    List<UpgradableData> _upgradeDatas;

    public SpawnBlackholeUpgrader(List<UpgradableData> upgradeDatas)
    {
        _upgradeDatas = upgradeDatas;
    }

    public void Visit(ISkillUpgradable upgradable, SpawnBlackholeData data) 
    {
        UpgradableData upgradeData = _upgradeDatas[upgradable.UpgradePoint - 1];
        data._lifetime += upgradeData._lifeTime;
        data._sizeMultiplier += upgradeData._range;
    }
}
