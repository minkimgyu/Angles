using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// StatikkData�� �Ϻ� �����͸� �Ҵ�Ǿ����
// ����?
// �׷��� ���� ������ ����ؼ� �Ϻ� �����͸� ���׷��̵� ��Ű�� ����� �����ؾ��Ѵ�.

// �溸�� ���� ũ�� --> ������ ���

[System.Serializable]
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

    [JsonProperty] List<UpgradableData> _upgradeDatas;

    public StatikkUpgrader(List<UpgradableData> upgradeDatas)
    {
        _upgradeDatas = upgradeDatas;
    }

    public StatikkUpgrader() { }

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

        data.Damage += upgradeData._damage;
        data.Range += upgradeData._range;
        data.MaxTargetCount += upgradeData._maxTargetCount;
        data.MaxStackCount += upgradeData._maxStackCount;

        Debug.Log("_damage: " + data.Damage);
        Debug.Log("_sizeMultiplier: " + data.Range);
        Debug.Log("_maxTargetCount: " + data.MaxTargetCount);
        Debug.Log("_maxStackCount: " + data.MaxStackCount);
    }
}
