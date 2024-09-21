using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TriangleData : EnemyData
{
    public float _moveSpeed;

    public TriangleData(float maxHp, ITarget.Type targetType, BaseLife.Size size, Dictionary<BaseSkill.Name, int> skillDataToAdd,
        DropData dropData, float moveSpeed) : base(maxHp, targetType, size, skillDataToAdd, dropData)
    {
        _moveSpeed = moveSpeed;
        _skillDataToAdd = skillDataToAdd;
    }

    public override LifeData Copy()
    {
        return new TriangleData(
            _maxHp, // EnemyData���� ��ӵ� ��
            _targetType, // EnemyData���� ��ӵ� ��
            _size, // EnemyData���� ��ӵ� ��
            new Dictionary<BaseSkill.Name, int>(_skillDataToAdd), // ��ųʸ� ���� ����
            _dropData, // EnemyData���� ��ӵ� ��
            _moveSpeed // TriangleData ���� ��
        );
    }
}

public class TriangleCreater : LifeCreater
{
    BaseFactory _skillFactory;

    public TriangleCreater(BaseLife lifePrefab, LifeData lifeData, BaseFactory effectFactory,
        BaseFactory skillFactory) : base(lifePrefab, lifeData, effectFactory)
    {
        _skillFactory = skillFactory;
    }

    public override BaseLife Create()
    {
        BaseLife life = UnityEngine.Object.Instantiate(_lifePrefab);
        if (life == null) return null;
        TriangleData data = _lifeData as TriangleData;

        // level ���� ����
        // ���⿡ Upgrader�� �־ �����͸� ������Ʈ �����ְ�
        // �Ʒ����� �ʱ�ȭ���ָ� ���?

        life.ResetData(data);
        life.AddEffectFactory(_effectFactory);

        life.Initialize();

        ISkillAddable skillUsable = life.GetComponent<ISkillAddable>();
        if (skillUsable == null) return life;

        foreach (var item in data._skillDataToAdd)
        {
            BaseSkill skill = _skillFactory.Create(item.Key);
            skill.Upgrade(item.Value);
            skillUsable.AddSkill(item.Key, skill);
        }

        return life;
    }
}
