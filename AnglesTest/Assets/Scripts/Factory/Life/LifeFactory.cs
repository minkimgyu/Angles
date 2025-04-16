using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using Skill;
using Newtonsoft.Json.Converters;

[System.Serializable]
abstract public class LifeData
{
    [JsonConstructor]
    public LifeData(UpgradeableStat<float> maxHp, ITarget.Type targetType, BaseEffect.Name destroyEffectName)
    {
        _maxHp = maxHp;
        _targetType = targetType;
        _destroyEffectName = destroyEffectName;

        _autoHpRecoveryPoint = new UpgradeableStat<float>(0);
        _damageReductionRatio = new UpgradeableStat<float>(0);
    }

    [JsonConstructor]
    public LifeData(UpgradeableStat<float> maxHp, ITarget.Type targetType, BaseEffect.Name destroyEffectName, UpgradeableStat<float> autoHpRecoveryPoint, UpgradeableStat<float> damageReductionRatio)
    {
        _maxHp = maxHp;
        _targetType = targetType;
        _destroyEffectName = destroyEffectName;

        _autoHpRecoveryPoint = autoHpRecoveryPoint;
        _damageReductionRatio = damageReductionRatio;
    }

    [JsonIgnore] public Action<float> OnMaxHpChanged;
    [JsonIgnore] public UpgradeableStat<float> MaxHp { get => _maxHp; set => _maxHp = value; }
    [JsonIgnore] public UpgradeableStat<float> AutoHpRecoveryPoint { get => _autoHpRecoveryPoint; set => _autoHpRecoveryPoint = value; }
    [JsonIgnore] public UpgradeableStat<float> DamageReductionRatio { get => _damageReductionRatio; set => _damageReductionRatio = value; }
    [JsonIgnore] public ITarget.Type TargetType { get => _targetType; set => _targetType = value; }
    [JsonIgnore] public BaseEffect.Name DestroyEffectName { get => _destroyEffectName; }

    [JsonProperty] protected UpgradeableStat<float> _maxHp;
    [JsonProperty] [JsonConverter(typeof(StringEnumConverter))] protected BaseEffect.Name _destroyEffectName;

    [JsonProperty] protected UpgradeableStat<float> _autoHpRecoveryPoint; // 일정 시간마다 체력 회복
    [JsonProperty] protected UpgradeableStat<float> _damageReductionRatio; // 데미지 감소 수치
    [JsonProperty] [JsonConverter(typeof(StringEnumConverter))] protected ITarget.Type _targetType;

    public abstract LifeData Copy();
}

[Serializable]
abstract public class EnemyData : LifeData
{
    [JsonProperty] protected BaseLife.Size _size;
    [JsonProperty] protected Dictionary<BaseSkill.Name, int> _skillData;

    [JsonIgnore] public BaseLife.Size Size { get => _size; set => _size = value; }
    [JsonIgnore] public Dictionary<BaseSkill.Name, int> SkillData { get => _skillData; set => _skillData = value; }

    public EnemyData(
        UpgradeableStat<float> maxHp,
        ITarget.Type targetType,
        BaseEffect.Name destroyEffectName,
        BaseLife.Size size,
        Dictionary<BaseSkill.Name, int> skillData) : base(maxHp, targetType, destroyEffectName)
    {
        _size = size;
        _skillData = skillData;
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

    public LifeFactory(Dictionary<BaseLife.Name, BaseLife> lifePrefabs, Dictionary<BaseLife.Name, LifeData> lifeDatas, Dictionary<BaseLife.Name, DropData> dropDatas,
        BaseFactory effectFactory, BaseFactory skillFactory)
    {
        _lifeCreaters = new Dictionary<BaseLife.Name, LifeCreater>();

        _lifeCreaters[BaseLife.Name.Player] = new PlayerCreater(lifePrefabs[BaseLife.Name.Player], lifeDatas[BaseLife.Name.Player], effectFactory, skillFactory);

        _lifeCreaters[BaseLife.Name.YellowTriangle] = new TriangleCreater(lifePrefabs[BaseLife.Name.YellowTriangle], lifeDatas[BaseLife.Name.YellowTriangle], dropDatas[BaseLife.Name.YellowTriangle], effectFactory, skillFactory);
        _lifeCreaters[BaseLife.Name.YellowRectangle] = new RectangleCreater(lifePrefabs[BaseLife.Name.YellowRectangle], lifeDatas[BaseLife.Name.YellowRectangle], dropDatas[BaseLife.Name.YellowRectangle], effectFactory, skillFactory);
        _lifeCreaters[BaseLife.Name.YellowPentagon] = new PentagonCreater(lifePrefabs[BaseLife.Name.YellowPentagon], lifeDatas[BaseLife.Name.YellowPentagon], dropDatas[BaseLife.Name.YellowPentagon], effectFactory, skillFactory);
        _lifeCreaters[BaseLife.Name.YellowHexagon] = new NormalHexagonCreater(lifePrefabs[BaseLife.Name.YellowHexagon], lifeDatas[BaseLife.Name.YellowHexagon], dropDatas[BaseLife.Name.YellowHexagon], effectFactory, skillFactory);

        _lifeCreaters[BaseLife.Name.RedTriangle] = new TriangleCreater(lifePrefabs[BaseLife.Name.RedTriangle], lifeDatas[BaseLife.Name.RedTriangle], dropDatas[BaseLife.Name.RedTriangle], effectFactory, skillFactory);
        _lifeCreaters[BaseLife.Name.RedRectangle] = new RectangleCreater(lifePrefabs[BaseLife.Name.RedRectangle], lifeDatas[BaseLife.Name.RedRectangle], dropDatas[BaseLife.Name.RedRectangle], effectFactory, skillFactory);
        _lifeCreaters[BaseLife.Name.RedPentagon] = new PentagonCreater(lifePrefabs[BaseLife.Name.RedPentagon], lifeDatas[BaseLife.Name.RedPentagon], dropDatas[BaseLife.Name.RedPentagon], effectFactory, skillFactory);
        _lifeCreaters[BaseLife.Name.RedHexagon] = new NormalHexagonCreater(lifePrefabs[BaseLife.Name.RedHexagon], lifeDatas[BaseLife.Name.RedHexagon], dropDatas[BaseLife.Name.RedHexagon], effectFactory, skillFactory);

        _lifeCreaters[BaseLife.Name.OperaTriangle] = new TriangleCreater(lifePrefabs[BaseLife.Name.OperaTriangle], lifeDatas[BaseLife.Name.OperaTriangle], dropDatas[BaseLife.Name.OperaTriangle], effectFactory, skillFactory);
        _lifeCreaters[BaseLife.Name.OperaRectangle] = new RectangleCreater(lifePrefabs[BaseLife.Name.OperaRectangle], lifeDatas[BaseLife.Name.OperaRectangle], dropDatas[BaseLife.Name.OperaRectangle], effectFactory, skillFactory);
        _lifeCreaters[BaseLife.Name.OperaPentagon] = new PentagonCreater(lifePrefabs[BaseLife.Name.OperaPentagon], lifeDatas[BaseLife.Name.OperaPentagon], dropDatas[BaseLife.Name.OperaPentagon], effectFactory, skillFactory);
        _lifeCreaters[BaseLife.Name.OperaHexagon] = new OperaHexagonCreater(lifePrefabs[BaseLife.Name.OperaHexagon], lifeDatas[BaseLife.Name.OperaHexagon], dropDatas[BaseLife.Name.OperaHexagon], effectFactory, skillFactory);

        _lifeCreaters[BaseLife.Name.GreenTriangle] = new TriangleCreater(lifePrefabs[BaseLife.Name.GreenTriangle], lifeDatas[BaseLife.Name.GreenTriangle], dropDatas[BaseLife.Name.GreenTriangle], effectFactory, skillFactory);
        _lifeCreaters[BaseLife.Name.GreenRectangle] = new RectangleCreater(lifePrefabs[BaseLife.Name.GreenRectangle], lifeDatas[BaseLife.Name.GreenRectangle], dropDatas[BaseLife.Name.GreenRectangle], effectFactory, skillFactory);
        _lifeCreaters[BaseLife.Name.GreenPentagon] = new GreenPentagonCreater(lifePrefabs[BaseLife.Name.GreenPentagon], lifeDatas[BaseLife.Name.GreenPentagon], dropDatas[BaseLife.Name.GreenPentagon], effectFactory, skillFactory);
        _lifeCreaters[BaseLife.Name.GreenHexagon] = new NormalHexagonCreater(lifePrefabs[BaseLife.Name.GreenHexagon], lifeDatas[BaseLife.Name.GreenHexagon], dropDatas[BaseLife.Name.GreenHexagon], effectFactory, skillFactory);

        _lifeCreaters[BaseLife.Name.Tricon] = new TriconCreater(lifePrefabs[BaseLife.Name.Tricon], lifeDatas[BaseLife.Name.Tricon], dropDatas[BaseLife.Name.Tricon], effectFactory, skillFactory);
        _lifeCreaters[BaseLife.Name.Rhombus] = new RhombusCreater(lifePrefabs[BaseLife.Name.Rhombus], lifeDatas[BaseLife.Name.Rhombus], dropDatas[BaseLife.Name.Rhombus], effectFactory, skillFactory);
        _lifeCreaters[BaseLife.Name.Pentagonic] = new PentagonicCreater(lifePrefabs[BaseLife.Name.Pentagonic], lifeDatas[BaseLife.Name.Pentagonic], dropDatas[BaseLife.Name.Pentagonic], effectFactory, skillFactory);
        _lifeCreaters[BaseLife.Name.Hexahorn] = new HexahornCreater(lifePrefabs[BaseLife.Name.Hexahorn], lifeDatas[BaseLife.Name.Hexahorn], dropDatas[BaseLife.Name.Hexahorn], effectFactory, skillFactory);
        _lifeCreaters[BaseLife.Name.Octavia] = new OctaviaCreater(lifePrefabs[BaseLife.Name.Octavia], lifeDatas[BaseLife.Name.Octavia], dropDatas[BaseLife.Name.Octavia], effectFactory, skillFactory);
        _lifeCreaters[BaseLife.Name.Hexatric] = new HexatricCreater(lifePrefabs[BaseLife.Name.Hexatric], lifeDatas[BaseLife.Name.Hexatric], dropDatas[BaseLife.Name.Hexatric], effectFactory, skillFactory);
    }

    public override BaseLife Create(BaseLife.Name name)
    {
        return _lifeCreaters[name].Create();
    }
}
