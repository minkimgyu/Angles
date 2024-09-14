using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// StatikkData�� �Ϻ� �����͸� �Ҵ�Ǿ����
// ����?
// �׷��� ���� ������ ����ؼ� �Ϻ� �����͸� ���׷��̵� ��Ű�� ����� �����ؾ��Ѵ�.

// �溸�� ���� ũ�� --> ������ ���

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
