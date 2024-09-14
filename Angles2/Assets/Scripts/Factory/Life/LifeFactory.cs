using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Drawing;

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
    public BaseEnemy.Size _size;
    public List<BaseSkill.Name> _skillNames;
    public DropData _dropData;

    public EnemyData(float maxHp, ITarget.Type targetType, BaseEnemy.Size size, List<BaseSkill.Name> skillNames, DropData dropData) : base(maxHp, targetType)
    {
        _size = size;
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

        _lifeCreaters[BaseLife.Name.YellowTriangle] = new TriangleCreater(lifePrefabs[BaseLife.Name.YellowTriangle], lifeDatas[BaseLife.Name.YellowTriangle], effectFactory, skillFactory);
        _lifeCreaters[BaseLife.Name.YellowRectangle] = new RectangleCreater(lifePrefabs[BaseLife.Name.YellowRectangle], lifeDatas[BaseLife.Name.YellowRectangle], effectFactory, skillFactory);
        _lifeCreaters[BaseLife.Name.YellowPentagon] = new PentagonCreater(lifePrefabs[BaseLife.Name.YellowPentagon], lifeDatas[BaseLife.Name.YellowPentagon], effectFactory, skillFactory);
        _lifeCreaters[BaseLife.Name.YellowHexagon] = new HexagonCreater(lifePrefabs[BaseLife.Name.YellowHexagon], lifeDatas[BaseLife.Name.YellowHexagon], effectFactory, skillFactory);

        _lifeCreaters[BaseLife.Name.RedTriangle] = new TriangleCreater(lifePrefabs[BaseLife.Name.RedTriangle], lifeDatas[BaseLife.Name.RedTriangle], effectFactory, skillFactory);
        _lifeCreaters[BaseLife.Name.RedRectangle] = new RectangleCreater(lifePrefabs[BaseLife.Name.RedRectangle], lifeDatas[BaseLife.Name.RedRectangle], effectFactory, skillFactory);
        _lifeCreaters[BaseLife.Name.RedPentagon] = new PentagonCreater(lifePrefabs[BaseLife.Name.RedPentagon], lifeDatas[BaseLife.Name.RedPentagon], effectFactory, skillFactory);
        _lifeCreaters[BaseLife.Name.RedHexagon] = new HexagonCreater(lifePrefabs[BaseLife.Name.RedHexagon], lifeDatas[BaseLife.Name.RedHexagon], effectFactory, skillFactory);
    }

    public override BaseLife Create(BaseLife.Name name)
    {
        return _lifeCreaters[name].Create();
    }
}
