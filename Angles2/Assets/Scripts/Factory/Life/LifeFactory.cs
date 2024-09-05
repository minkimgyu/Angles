using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class BaseLifeData
{
    public BaseLifeData(float maxHp, ITarget.Type targetType)
    {
        _maxHp = maxHp;
        _targetType = targetType;
    }

    public float _maxHp;
    public ITarget.Type _targetType;
}

[Serializable]
public class EnemyData : BaseLifeData
{
    public List<BaseSkill.Name> _skillNames;
    public DropData _dropData;

    public EnemyData(float maxHp, ITarget.Type targetType, List<BaseSkill.Name> skillNames, DropData dropData) : base(maxHp, targetType)
    {
        _skillNames = skillNames;
        _dropData = dropData;
    }
}

abstract public class LifeCreater
{
    protected BaseLife _lifePrefab;
    protected BaseLifeData _lifeData;
    protected BaseFactory _effectFactory;

    public LifeCreater(BaseLife lifePrefab, BaseLifeData lifeData, BaseFactory effectFactory) 
    { _lifePrefab = lifePrefab; _lifeData = lifeData; _effectFactory = effectFactory; }

    public abstract BaseLife Create();
}

public class LifeFactory : BaseFactory
{
    Dictionary<BaseLife.Name, LifeCreater> _lifeCreaters;

    public LifeFactory(Dictionary<BaseLife.Name, BaseLife> lifePrefabs, Dictionary<BaseLife.Name, BaseLifeData> lifeDatas,
        BaseFactory effectFactory, BaseFactory skillFactory)
    {
        _lifeCreaters = new Dictionary<BaseLife.Name, LifeCreater>();

        _lifeCreaters[BaseLife.Name.Player] = new PlayerCreater(lifePrefabs[BaseLife.Name.Player], lifeDatas[BaseLife.Name.Player], effectFactory, skillFactory);
        _lifeCreaters[BaseLife.Name.Triangle] = new TriangleCreater(lifePrefabs[BaseLife.Name.Triangle], lifeDatas[BaseLife.Name.Triangle], effectFactory, skillFactory);
        _lifeCreaters[BaseLife.Name.Rectangle] = new RectangleCreater(lifePrefabs[BaseLife.Name.Rectangle], lifeDatas[BaseLife.Name.Rectangle], effectFactory, skillFactory);
        _lifeCreaters[BaseLife.Name.Pentagon] = new PentagonCreater(lifePrefabs[BaseLife.Name.Pentagon], lifeDatas[BaseLife.Name.Pentagon], effectFactory, skillFactory);
        _lifeCreaters[BaseLife.Name.Hexagon] = new HexagonCreater(lifePrefabs[BaseLife.Name.Hexagon], lifeDatas[BaseLife.Name.Hexagon], effectFactory, skillFactory);
    }

    public override BaseLife Create(BaseLife.Name name)
    {
        return _lifeCreaters[name].Create();
    }
}
