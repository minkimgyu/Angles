using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    List<UpgradableData> _upgradeDatas;

    public SpawnBlackholeUpgrader(List<UpgradableData> upgradeDatas)
    {
        _upgradeDatas = upgradeDatas;
    }

    public void Visit(ISkillUpgradable upgradable, SpawnBlackholeData data) 
    {
        // 2랩부터 업그레이드가 들어가기 때문에 이렇게 한다.
        int index = ReturnUpgradeDataIndex(upgradable.UpgradePoint);
        UpgradableData upgradeData = _upgradeDatas[index];

        data._lifetime += upgradeData._lifeTime;
        data._sizeMultiplier += upgradeData._range;
    }
}
