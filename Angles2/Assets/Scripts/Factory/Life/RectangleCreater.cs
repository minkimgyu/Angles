using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RectangleData : BaseLifeData
{
    public float _moveSpeed;
    public List<BaseSkill.Name> _skillNames;

    public RectangleData(float maxHp, ITarget.Type targetType,
        float moveSpeed, List<BaseSkill.Name> skillNames) : base(maxHp, targetType)
    {
        _moveSpeed = moveSpeed;
        _skillNames = skillNames;
    }
}

public class RectangleCreater : LifeCreater<RectangleData>
{
    public override BaseLife Create()
    {
        BaseLife life = Object.Instantiate(_prefab);
        life.ResetData(_data);
        life.Initialize();

        return life;
    }
}