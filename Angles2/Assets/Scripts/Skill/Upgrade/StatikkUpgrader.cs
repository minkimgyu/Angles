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
        public UpgradableData(float damage, float range, int maxTargetCount)
        {
            _damage = damage;
            _range = range;
            _maxTargetCount = maxTargetCount;
        }

        private float _damage;
        private float _range;
        private int _maxTargetCount;

        public float Damage { get => _damage; }
        public float Range { get => _range; }
        public int MaxTargetCount { get => _maxTargetCount; }
    }

    List<UpgradableData> _upgradeDatas;

    public StatikkUpgrader(List<UpgradableData> upgradeDatas)
    {
        _upgradeDatas = upgradeDatas;
    }

    public void Visit(IUpgradable upgradable, StatikkData data)
    {
        UpgradableData upgradeData = _upgradeDatas[upgradable.UpgradePoint - 1];
        data += upgradeData;
    }
}
