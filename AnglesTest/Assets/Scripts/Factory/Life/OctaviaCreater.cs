using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skill;

[System.Serializable]
public class OctaviaData : EnemyData
{
    [JsonProperty] private float _moveSpeed;
    [JsonProperty] private float _stopDistance;
    [JsonProperty] private float _gap;

    [JsonIgnore] public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }
    [JsonIgnore] public float StopDistance { get => _stopDistance; set => _stopDistance = value; }
    [JsonIgnore] public float Gap { get => _gap; set => _gap = value; }

    public OctaviaData(UpgradeableStat<float> maxHp, ITarget.Type targetType, BaseEffect.Name dieEffectName, BaseLife.Size size, Dictionary<BaseSkill.Name, int> skillDataToAdd,
        float moveSpeed, float stopDistance, float gap) : base(maxHp, targetType, dieEffectName, size, skillDataToAdd)
    {
        _moveSpeed = moveSpeed;
        _stopDistance = stopDistance;
        _gap = gap;
    }

    public override LifeData Copy()
    {
        return new OctaviaData(
            _maxHp.Copy(), // EnemyData���� ��ӵ� ��
            _targetType, // EnemyData���� ��ӵ� ��
            _destroyEffectName,
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

        life.InjectData(data, _dropData);
        life.InjectEffectFactory(_effectFactory);

        life.Initialize();

        ICaster caster = life.GetComponent<ICaster>();
        if (caster == null) return life;

        foreach (var item in data.SkillData)
        {
            for (int i = 0; i <= item.Value; i++)
            {
                BaseSkill skill = _skillFactory.Create(item.Key);
                caster.AddSkill(item.Key, skill);
            }
        }

        return life;
    }
}
