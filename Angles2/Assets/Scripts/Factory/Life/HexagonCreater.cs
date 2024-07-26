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

public class HexagonCreater : LifeCreater
{
    public override BaseLife Create()
    {
        GameObject obj = Object.Instantiate(_prefab);
        BaseLife life = obj.GetComponent<HexagonEnemy>();
        if (life == null) return null;

        HexagonData data = Database.Instance.LifeDatas[BaseLife.Name.Hexagon] as HexagonData;
        life.ResetData(data);
        life.Initialize();

        return life;
    }
}
