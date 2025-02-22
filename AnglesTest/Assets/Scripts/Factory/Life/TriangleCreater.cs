using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

[Serializable]
public class TriangleData : EnemyData
{
    [JsonProperty] private float _moveSpeed;

    public TriangleData(float maxHp, ITarget.Type targetType, BaseEffect.Name dieEffectName, BaseLife.Size size, Dictionary<BaseSkill.Name, int> skillDataToAdd, float moveSpeed) 
        : base(maxHp, targetType, dieEffectName,size, skillDataToAdd)
    {
        _moveSpeed = moveSpeed;
        _skillData = skillDataToAdd;
    }

    [JsonIgnore] public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }

    public override LifeData Copy()
    {
        return new TriangleData(
            _maxHp, // EnemyData���� ��ӵ� ��
            _targetType, // EnemyData���� ��ӵ� ��
            _destroyEffectName,
            _size, // EnemyData���� ��ӵ� ��
            new Dictionary<BaseSkill.Name, int>(_skillData), // ��ųʸ� ���� ����
            _moveSpeed // TriangleData ���� ��
        );
    }
}

public class TriangleCreater : LifeCreater
{
    BaseFactory _skillFactory;
    DropData _dropData;

    public TriangleCreater(BaseLife lifePrefab, LifeData lifeData, DropData dropData, BaseFactory effectFactory,
        BaseFactory skillFactory) : base(lifePrefab, lifeData, effectFactory)
    {
        _skillFactory = skillFactory;
        _dropData = dropData;
    }

    public override BaseLife Create()
    {
        BaseLife life = UnityEngine.Object.Instantiate(_lifePrefab);
        if (life == null) return null;
        TriangleData data = CopyLifeData as TriangleData;

        // level ���� ����
        // ���⿡ Upgrader�� �־ �����͸� ������Ʈ �����ְ�
        // �Ʒ����� �ʱ�ȭ���ָ� ���?

        life.InjectData(data, _dropData);
        life.InjectEffectFactory(_effectFactory);

        life.Initialize();

        ICaster caster = life.GetComponent<ICaster>();
        if (caster == null) return life;

        foreach (var item in data.SkillData)
        {
            BaseSkill skill = _skillFactory.Create(item.Key);
            skill.Upgrade(item.Value);
            caster.AddSkill(item.Key, skill);
        }

        return life;
    }
}
