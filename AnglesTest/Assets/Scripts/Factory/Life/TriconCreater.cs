using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TriconData : EnemyData
{
    public float _moveSpeed;
    public float _stopDistance;
    public float _gap;

    public float _freezeDuration;
    public float _movableDuration;

    public TriconData(float maxHp, ITarget.Type targetType, BaseLife.Size size, Dictionary<BaseSkill.Name, int> skillDataToAdd,
        DropData dropData, float moveSpeed, float stopDistance, float gap, float freezeDuration, float movableDuration) : base(maxHp, targetType, size, skillDataToAdd, dropData)
    {
        _moveSpeed = moveSpeed;
        _stopDistance = stopDistance;
        _gap = gap;

        _freezeDuration = freezeDuration;
        _movableDuration = movableDuration;
    }

    public override LifeData Copy()
    {
        return new TriconData(
            _maxHp, // EnemyData���� ��ӵ� ��
            _targetType, // EnemyData���� ��ӵ� ��
            _size, // EnemyData���� ��ӵ� ��
            new Dictionary<BaseSkill.Name, int>(CopySkillDataToAdd), // ��ųʸ� ���� ����
            _dropData, // EnemyData���� ��ӵ� ��
            _moveSpeed, // TriangleData ���� ��
            _stopDistance,
            _gap,
            _freezeDuration,
            _movableDuration
        );
    }
}

public class TriconCreater : LifeCreater
{
    BaseFactory _skillFactory;

    public TriconCreater(BaseLife lifePrefab, LifeData lifeData, BaseFactory SpawnEffect,
        BaseFactory skillFactory) : base(lifePrefab, lifeData, SpawnEffect)
    {
        _skillFactory = skillFactory;
    }

    public override BaseLife Create()
    {
        BaseLife life = UnityEngine.Object.Instantiate(_lifePrefab);
        if (life == null) return null;

        TriconData data = CopyLifeData as TriconData;

        life.ResetData(data);
        life.AddEffectFactory(_effectFactory);

        life.Initialize();

        ICaster skillAddable = life.GetComponent<ICaster>();
        if (skillAddable == null) return life;

        foreach (var item in data.CopySkillDataToAdd)
        {
            BaseSkill skill = _skillFactory.Create(item.Key);
            skill.Upgrade(item.Value);
            skillAddable.AddSkill(item.Key, skill);
        }

        return life;
    }
}
