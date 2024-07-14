using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TriangleData : BaseLifeData
{
    public float _moveSpeed;
    public List<BaseSkill.Name> _skillNames;

    public TriangleData(float maxHp, ITarget.Type targetType,
        float moveSpeed, List<BaseSkill.Name> skillNames) : base(maxHp, targetType)
    {
        _moveSpeed = moveSpeed;
        _skillNames = skillNames;
    }
}

public class TriangleCreater : LifeCreater<TriangleData>
{
    IPos _followTarget;

    public override void Initialize(LifeCreaterInput input)
    {
        base.Initialize(input);
    }

    public override BaseLife Create()
    {
        BaseLife life = Object.Instantiate(_prefab);
        life.ResetData(_data);
        life.Initialize();

        return life;
    }
}
