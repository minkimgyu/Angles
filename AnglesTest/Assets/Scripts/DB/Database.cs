using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public struct Database
{
    #region SKIN 데이터

    [JsonProperty]
    Dictionary<SkinData.Key, List<IStatModifier>> _skinModifiers;

    [JsonIgnore]
    public Dictionary<SkinData.Key, List<IStatModifier>> SkinModifiers { get { return _skinModifiers; } }

    [JsonProperty]
    Dictionary<SkinData.Key, SkinData> _skinDatas;

    [JsonIgnore]
    public Dictionary<SkinData.Key, SkinData> SkinDatas { get { return _skinDatas; } }

    #endregion

    #region STAT 데이터

    [JsonProperty]
    Dictionary<StatData.Key, IStatModifier> _statModifiers;

    [JsonIgnore]
    public Dictionary<StatData.Key, IStatModifier> StatModifiers { get { return _statModifiers; } }

    [JsonProperty]
    Dictionary<StatData.Key, StatData> _statDatas;

    [JsonIgnore]
    public Dictionary<StatData.Key, StatData> StatDatas { get { return _statDatas; } }

    #endregion

    #region SKILL 데이터

    [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
    HashSet<BaseSkill.Name> _upgradeableSkills;

    [JsonIgnore]
    public HashSet<BaseSkill.Name> UpgradeableSkills { get { return _upgradeableSkills; } }

    [JsonProperty]
    Dictionary<BaseSkill.Name, IUpgradeVisitor> _upgrader;

    [JsonIgnore]
    public Dictionary<BaseSkill.Name, IUpgradeVisitor> Upgraders { get { return _upgrader; } }

    [JsonProperty]
    Dictionary<BaseSkill.Name, SkillData> _skillDatas;

    [JsonIgnore]
    public Dictionary<BaseSkill.Name, SkillData> SkillDatas { get { return _skillDatas; } }

    #endregion

    #region WEAPON 데이터
    [JsonProperty]
    Dictionary<BaseWeapon.Name, WeaponData> _weaponDatas;

    [JsonIgnore]
    public Dictionary<BaseWeapon.Name, WeaponData> WeaponDatas { get { return _weaponDatas; } }

    #endregion

    #region DROP 데이터

    [JsonProperty]
    Dictionary<BaseLife.Name, DropData> _chapterDropDatas;

    [JsonIgnore]
    public Dictionary<BaseLife.Name, DropData> ChapterDropDatas { get { return _chapterDropDatas; } }

    #endregion

    #region LIFE 데이터

    [JsonProperty]
    Dictionary<BaseLife.Name, LifeData> _lifeDatas;

    [JsonIgnore]
    public Dictionary<BaseLife.Name, LifeData> LifeDatas { get { return _lifeDatas; } }

    #endregion

    #region CARD 데이터
    [JsonProperty]
    Dictionary<BaseSkill.Name, CardInfoData> _cardDatas;

    [JsonIgnore]
    public Dictionary<BaseSkill.Name, CardInfoData> CardDatas { get { return _cardDatas; } }

    #endregion

    #region INTERACTABLE 데이터
    [JsonProperty]
    Dictionary<IInteractable.Name, BaseInteractableObjectData> _interactableObjectDatas;

    [JsonIgnore]
    public Dictionary<IInteractable.Name, BaseInteractableObjectData> InteractableObjectDatas { get { return _interactableObjectDatas; } }

    #endregion

    #region LEVEL 데이터

    [JsonProperty]
    Dictionary<GameMode.Level, ILevelInfo> _levelDatas;

    [JsonIgnore]
    public Dictionary<GameMode.Level, ILevelInfo> LevelDatas { get { return _levelDatas; } }

    #endregion

    #region CoinGauge 데이터

    [JsonProperty]
    List<int> _coinGaugeData;

    [JsonIgnore]
    public List<int> CoingaugeData { get { return _coinGaugeData; } }

    #endregion

    public Database(
        Dictionary<SkinData.Key, List<IStatModifier>> skinModifiers,
        Dictionary<SkinData.Key, SkinData> skinDatas,
        Dictionary<StatData.Key, IStatModifier> statModifiers,
        Dictionary<StatData.Key, StatData> statDatas,
        HashSet<BaseSkill.Name> upgradeableSkills,
        Dictionary<BaseSkill.Name, IUpgradeVisitor> upgrader,
        Dictionary<BaseSkill.Name, SkillData> skillDatas,
        Dictionary<BaseWeapon.Name, WeaponData> weaponDatas,
        Dictionary<BaseLife.Name, DropData> chapterDropDatas,
        Dictionary<BaseLife.Name, LifeData> lifeDatas,
        Dictionary<BaseSkill.Name, CardInfoData> cardDatas,
        Dictionary<IInteractable.Name, BaseInteractableObjectData> interactableObjectDatas,
        Dictionary<GameMode.Level, ILevelInfo> levelDatas,
        List<int> coinGaugeData)
    {
        _skinModifiers = skinModifiers;
        _skinDatas = skinDatas;
        _statModifiers = statModifiers;
        _statDatas = statDatas;
        _upgradeableSkills = upgradeableSkills;
        _upgrader = upgrader;
        _skillDatas = skillDatas;
        _weaponDatas = weaponDatas;
        _chapterDropDatas = chapterDropDatas;
        _lifeDatas = lifeDatas;
        _cardDatas = cardDatas;
        _interactableObjectDatas = interactableObjectDatas;
        _levelDatas = levelDatas;
        _coinGaugeData = coinGaugeData;
    }
}
