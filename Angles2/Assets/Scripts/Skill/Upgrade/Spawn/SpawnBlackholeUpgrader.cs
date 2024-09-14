using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlackholeUpgrader : IUpgradeVisitor
{
    public struct UpgradableData
    {
        public UpgradableData(float lifeTime, float absorbForce, int maxTargetCount, float range)
        {
            LifeTime = lifeTime;
            AbsorbForce = absorbForce;
            MaxTargetCount = maxTargetCount;
            Range = range;
        }

        public float LifeTime { get; private set; }
        public float AbsorbForce { get; private set; }
        public int MaxTargetCount { get; private set; }
        public float Range { get; private set; }
    }

    List<UpgradableData> _upgradeDatas;

    public SpawnBlackholeUpgrader(List<UpgradableData> upgradeDatas)
    {
        _upgradeDatas = upgradeDatas;
    }

    public void Visit(IUpgradable upgradable, BlackholeData data) 
    {
        UpgradableData upgradeData = _upgradeDatas[upgradable.UpgradePoint - 1];
        data += upgradeData;
    }
}
