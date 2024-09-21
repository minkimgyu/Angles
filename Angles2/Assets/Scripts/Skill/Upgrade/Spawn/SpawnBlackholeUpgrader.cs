using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlackholeUpgrader : IUpgradeVisitor
{
    public struct UpgradableData
    {
        public UpgradableData(float lifeTime, float absorbForce, int maxTargetCount, float range)
        {
            _lifeTime = lifeTime;
            _absorbForce = absorbForce;
            _maxTargetCount = maxTargetCount;
            _range = range;
        }

        public float _lifeTime;
        public float _absorbForce;
        public int _maxTargetCount;
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
        data._force += upgradeData._absorbForce;
        data._targetCount += upgradeData._maxTargetCount;
        data._sizeMultiplier += upgradeData._range;
    }
}
