using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactUpgrader : BaseSkillUpgrader, IUpgradeVisitor
{
    public struct UpgradableData
    {
        public UpgradableData(float damage, float range)
        {
            _damage = damage;
            _range = range;
        }

        public float _damage;
        public float _range;
    }

    List<UpgradableData> _upgradeDatas;

    public ImpactUpgrader(List<UpgradableData> upgradeDatas)
    {
        _upgradeDatas = upgradeDatas;
    }

    public void Visit(ISkillUpgradable upgradable, ImpactData data)
    {
        // 2랩부터 업그레이드가 들어가기 때문에 이렇게 한다.
        int index = ReturnUpgradeDataIndex(upgradable.UpgradePoint);
        UpgradableData upgradeData = _upgradeDatas[index];

        data._damage += upgradeData._damage;
        data._rangeMultiplier += upgradeData._range;

        Debug.Log("_damage: " + data._damage);
        Debug.Log("_rangeMultiplier: " + data._rangeMultiplier);
    }
}
