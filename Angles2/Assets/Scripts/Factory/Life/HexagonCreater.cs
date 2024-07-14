using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HexagonData : BaseLifeData
{
    public float _moveSpeed;
    public List<BaseSkill.Name> _skillNames;
    public float _stopDistance;
    public float _gap;

    public HexagonData(float maxHp, ITarget.Type targetType,
        float moveSpeed, List<BaseSkill.Name> skillNames, float stopDistance, float gap) : base(maxHp, targetType)
    {
        _moveSpeed = moveSpeed;
        _skillNames = skillNames;

        _stopDistance = stopDistance;
        _gap = gap;
    }
}

public class HexagonCreater : LifeCreater<HexagonData>
{
    public override BaseLife Create()
    {
        BaseLife life = Object.Instantiate(_prefab);
        life.ResetData(_data);
        life.Initialize();

        return life;
    }
}
