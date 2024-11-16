using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
abstract public class LifeData
{
    public LifeData(float maxHp, ITarget.Type targetType, float autoHpRecoveryPoint = 0, float damageReductionRatio = 0)
    {
        _maxHp = maxHp;
        _hp = maxHp;
        _targetType = targetType;

        _autoHpRecoveryPoint = autoHpRecoveryPoint;
        _damageReductionRatio = damageReductionRatio;
    }

    // setter 활용
    public float MaxHp
    {
        get {  return _maxHp; } 
        set
        {
            float addPoint = value - _maxHp;
            _maxHp = value;
            _hp += addPoint;  // 기존 체력에 최대 체력 추가치를 더해준다.
        }
    }

    protected float _maxHp;
    public float _hp;

    public float _autoHpRecoveryPoint; // 일정 시간마다 체력 회복
    public float _damageReductionRatio; // 데미지 감소 수치
    public ITarget.Type _targetType;

    public abstract LifeData Copy();
}

[Serializable]
abstract public class EnemyData : LifeData
{
    public int _level;
    public BaseLife.Size _size;
    public Dictionary<BaseSkill.Name, int> CopySkillDataToAdd;
    public DropData _dropData;

    public EnemyData(
        float maxHp,
        ITarget.Type targetType,
        BaseLife.Size size,
        Dictionary<BaseSkill.Name, int> skillDataToAdd,
        DropData dropData) : base(maxHp, targetType)
    {
        _size = size;
        CopySkillDataToAdd = skillDataToAdd;
        _dropData = dropData;
    }
}

abstract public class LifeCreater
{
    protected LifeData CopyLifeData { get { return _lifeData.Copy(); } }

    protected BaseLife _lifePrefab;
    private LifeData _lifeData;
    protected BaseFactory _effectFactory;

    public LifeCreater(BaseLife lifePrefab, LifeData lifeData, BaseFactory effectFactory) 
    { 
        _lifePrefab = lifePrefab;
        _lifeData = lifeData; // 복사본을 준다.
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

        _lifeCreaters[BaseLife.Name.Tricon] = new TriconCreater(lifePrefabs[BaseLife.Name.Tricon], lifeDatas[BaseLife.Name.Tricon], effectFactory, skillFactory);
        _lifeCreaters[BaseLife.Name.Rhombus] = new RhombusCreater(lifePrefabs[BaseLife.Name.Rhombus], lifeDatas[BaseLife.Name.Rhombus], effectFactory, skillFactory);
        _lifeCreaters[BaseLife.Name.Pentagonic] = new PentagonicCreater(lifePrefabs[BaseLife.Name.Pentagonic], lifeDatas[BaseLife.Name.Pentagonic], effectFactory, skillFactory);
    }

    public override BaseLife Create(BaseLife.Name name)
    {
        return _lifeCreaters[name].Create();
    }
}
