using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// StatikkData�� �Ϻ� �����͸� �Ҵ�Ǿ����
// ����?
// �׷��� ���� ������ ����ؼ� �Ϻ� �����͸� ���׷��̵� ��Ű�� ����� �����ؾ��Ѵ�.

// �溸�� ���� ũ�� --> ������ ���

public class StatikkUpgrader : BaseSkillUpgrader, IUpgradeVisitor
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

    const int _upgradeOffset = 2;

    //1 - ó���� 1 --> ó���� ���׷��̵� X
    //2 - ���׷��̵�
    //3 - ���׷��̵�
    //4 - ���׷��̵�
    //5 - ���׷��̵�

    //1 - ���׷��̵�
    //2 - ���׷��̵�
    //3 - ���׷��̵�

    public void Visit(ISkillUpgradable upgradable, StatikkData data)
    {
        // 2������ ���׷��̵尡 ���� ������ �̷��� �Ѵ�.
        int index = ReturnUpgradeDataIndex(upgradable.UpgradePoint);
        UpgradableData upgradeData = _upgradeDatas[index];

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
