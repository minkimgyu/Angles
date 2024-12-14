using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackUpgrader : BaseSkillUpgrader, IUpgradeVisitor
{
    public struct UpgradableData
    {
        public UpgradableData(float damage, float sizeMultiplier)
        {
            _damage = damage;
            _sizeMultiplier = sizeMultiplier;
        }

        public float _damage;
        public float _sizeMultiplier;
    }

    List<UpgradableData> _upgradeDatas;

    public KnockbackUpgrader(List<UpgradableData> upgradeDatas)
    {
        _upgradeDatas = upgradeDatas;
    }

    const int _upgradeOffset = 2;

    public void Visit(ISkillUpgradable upgradable, KnockbackData data)
    {
        // 2랩부터 업그레이드가 들어가기 때문에 이렇게 한다.
        int index = ReturnUpgradeDataIndex(upgradable.UpgradePoint);
        UpgradableData upgradeData = _upgradeDatas[index];

        data._damage += upgradeData._damage;
        data._rangeMultiplier += upgradeData._sizeMultiplier;

        Debug.Log("_damage: " + data._damage);
        Debug.Log("_rangeMultiplier: " + data._rangeMultiplier);
    }
}
