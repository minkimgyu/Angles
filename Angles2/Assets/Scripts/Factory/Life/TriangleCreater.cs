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

public class TriangleCreater : LifeCreater
{
    public override BaseLife Create()
    {
        GameObject obj = Object.Instantiate(_prefab);
        BaseLife life = obj.GetComponent<TriangleEnemy>();
        if (life == null) return null;

        TriangleData playerData = Database.Instance.LifeDatas[BaseLife.Name.Triangle] as TriangleData;
        life.ResetData(playerData);
        life.Initialize();

        return life;
    }
}
