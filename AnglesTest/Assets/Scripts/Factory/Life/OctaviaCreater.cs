using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OctaviaData : EnemyData
{
    [JsonProperty] private float _moveSpeed;
    [JsonProperty] private float _stopDistance;
    [JsonProperty] private float _gap;

    [JsonIgnore] public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }
    [JsonIgnore] public float StopDistance { get => _stopDistance; set => _stopDistance = value; }
    [JsonIgnore] public float Gap { get => _gap; set => _gap = value; }

    public OctaviaData(float maxHp, ITarget.Type targetType, BaseLife.Size size, Dictionary<BaseSkill.Name, int> skillDataToAdd,
        float moveSpeed, float stopDistance, float gap) : base(maxHp, targetType, size, skillDataToAdd)
    {
        _moveSpeed = moveSpeed;
        _stopDistance = stopDistance;
        _gap = gap;
    }

    public override LifeData Copy()
    {
        return new OctaviaData(
            _maxHp, // EnemyData���� ��ӵ� ��
            _targetType, // EnemyData���� ��ӵ� ��
            _size, // EnemyData���� ��ӵ� ��
            new Dictionary<BaseSkill.Name, int>(_skillData), // ��ųʸ� ���� ����
            _moveSpeed, // TriangleData ���� ��
            _stopDistance,
            _gap
        );
    }
}

public class OctaviaCreater : LifeCreater
{
    BaseFactory _skillFactory;
    DropData _dropData;

    public OctaviaCreater(BaseLife lifePrefab, LifeData lifeData, DropData dropData, BaseFactory SpawnEffect,
        BaseFactory skillFactory) : base(lifePrefab, lifeData, SpawnEffect)
    {
        _skillFactory = skillFactory;
        _dropData = dropData;
    }

    public override BaseLife Create()
    {
        BaseLife life = UnityEngine.Object.Instantiate(_lifePrefab);
        if (life == null) return null;

        OctaviaData data = CopyLifeData as OctaviaData;

        life.ResetData(data, _dropData);
        life.AddEffectFactory(_effectFactory);

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
