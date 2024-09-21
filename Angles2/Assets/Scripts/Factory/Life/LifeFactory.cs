using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Drawing;
using UnityEditor.Experimental.GraphView;

[System.Serializable]
abstract public class LifeData
{
    public LifeData(float maxHp, ITarget.Type targetType)
    {
        _maxHp = maxHp;
        _targetType = targetType;
    }

    public float _maxHp;
    public ITarget.Type _targetType;

    public abstract LifeData Copy();
}

[Serializable]
abstract public class EnemyData : LifeData
{
    public int _level;
    public BaseLife.Size _size;
    public Dictionary<BaseSkill.Name, int> _skillDataToAdd;
    public DropData _dropData;

    public EnemyData(
        float maxHp,
        ITarget.Type targetType,
        BaseLife.Size size,
        Dictionary<BaseSkill.Name, int> skillDataToAdd,
        DropData dropData) : base(maxHp, targetType)
    {
        _size = size;
        _skillDataToAdd = skillDataToAdd;
        _dropData = dropData;
    }
}

abstract public class LifeCreater
{
    protected BaseLife _lifePrefab;
    protected LifeData _lifeData;
    protected BaseFactory _effectFactory;

    public LifeCreater(BaseLife lifePrefab, LifeData lifeData, BaseFactory effectFactory) 
    { 
        _lifePrefab = lifePrefab; 
        _lifeData = lifeData.Copy(); // 복사본을 준다.
        _effectFactory = effectFactory; 
    }

    public abstract BaseLife Create();
}

public class LifeFactory : BaseFactory
{
    Dictionary<BaseLife.Name, LifeCreater> _lifeCreaters;

    public LifeFactory(Dictionary<BaseLife.Name, BaseLife> lifePrefabs, Dictionary<BaseLife.Name, LifeData> lifeDatas,
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

        _lifeCreaters[BaseLife.Name.Lombard] = new LombardCreater(lifePrefabs[BaseLife.Name.Lombard], lifeDatas[BaseLife.Name.Lombard], effectFactory, skillFactory);
    }

    public override BaseLife Create(BaseLife.Name name)
    {
        return _lifeCreaters[name].Create();
    }
}
