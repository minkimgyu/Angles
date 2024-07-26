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

public class RectangleCreater : LifeCreater
{
    public override BaseLife Create()
    {
        GameObject obj = Object.Instantiate(_prefab);
        BaseLife life = obj.GetComponent<RectangleEnemy>();
        if (life == null) return null;

        RectangleData data = Database.Instance.LifeDatas[BaseLife.Name.Rectangle] as RectangleData;
        life.ResetData(data);
        life.Initialize();

        return life;
    }
}