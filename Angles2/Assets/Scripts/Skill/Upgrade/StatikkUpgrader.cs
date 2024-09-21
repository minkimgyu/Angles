using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// StatikkData의 일부 데이터만 할당되어야함
// 맞지?
// 그래서 빌더 패턴을 사용해서 일부 데이터만 업그레이드 시키는 방법을 제공해야한다.

// 득보다 실이 크다 --> 생성자 사용

public class StatikkUpgrader : IUpgradeVisitor
{
    public struct UpgradableData
    {
        public UpgradableData(float damage, float range, int maxTargetCount, int maxStackCount)
        {
            _damage = damage;
            _range = range;
            _maxTargetCount = maxTargetCount;
            _maxStackCount = maxStackCount;

        }

        public float _damage;
        public float _range;
        public int _maxTargetCount;
        public int _maxStackCount;
    }

    List<UpgradableData> _upgradeDatas;

    public StatikkUpgrader(List<UpgradableData> upgradeDatas)
    {
        _upgradeDatas = upgradeDatas;
    }

    public void Visit(ISkillUpgradable upgradable, StatikkData data)
    {
        UpgradableData upgradeData = _upgradeDatas[upgradable.UpgradePoint - 1];

        data._damage += upgradeData._damage;
        data._range += upgradeData._range;
        data._maxTargetCount += upgradeData._maxTargetCount;
        data._maxStackCount += upgradeData._maxStackCount;

        Debug.Log("_damage: " + data._damage);
        Debug.Log("_sizeMultiplier: " + data._range);
        Debug.Log("_maxTargetCount: " + data._maxTargetCount);
        Debug.Log("_maxStackCount: " + data._maxStackCount);
    }
}
