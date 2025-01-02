using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

[Serializable]
abstract public class SkillData
{
    [JsonProperty] protected int _maxUpgradePoint;

    public SkillData(int maxUpgradePoint)
    {
        _maxUpgradePoint = maxUpgradePoint;
    }

    public int MaxUpgradePoint { get => _maxUpgradePoint; set => _maxUpgradePoint = value; }

    public abstract SkillData Copy();
}

[Serializable]
abstract public class RandomSkillData : SkillData
{
    [JsonProperty] protected float _probability;

    public RandomSkillData(int maxUpgradePoint, float probability) : base(maxUpgradePoint)
    {
        _probability = probability;
    }

    public float Probability { get => _probability; set => _probability = value; }
}

[Serializable]
abstract public class CooltimeSkillData : SkillData
{
    [JsonProperty] protected float _coolTime;
    [JsonProperty] protected int _maxStackCount;

    [JsonIgnore] public int MaxStackCount { get => _maxStackCount; set => _maxStackCount = value; }
    [JsonIgnore] public float CoolTime { get => _coolTime; set => _coolTime = value; }

    public CooltimeSkillData(int maxUpgradePoint, float coolTime, int maxStackCount) : base(maxUpgradePoint)
    {
        _coolTime = coolTime;
        _maxStackCount = maxStackCount;
    }
}

abstract public class SkillCreater
{
    private SkillData _skillData;
    public SkillCreater(SkillData data) { _skillData = data; } // 생성자에 이팩트 생성을 받는 이벤트를 추가해준다.

    protected SkillData CopySkillData { get { return _skillData.Copy(); } }
    public abstract BaseSkill Create();
}

public class SkillFactory : BaseFactory
{
    Dictionary<BaseSkill.Name, SkillCreater> _skillCreaters;

    public SkillFactory(Dictionary<BaseSkill.Name, SkillData> skillDatas, Dictionary<BaseSkill.Name, IUpgradeVisitor> upgraders, BaseFactory effectFactory, BaseFactory weaponFactory)
    {
        _skillCreaters = new Dictionary<BaseSkill.Name, SkillCreater>();

        _skillCreaters[BaseSkill.Name.Statikk] = new StatikkCreater(skillDatas[BaseSkill.Name.Statikk], upgraders[BaseSkill.Name.Statikk], effectFactory);
        _skillCreaters[BaseSkill.Name.Knockback] = new KnockbackCreater(skillDatas[BaseSkill.Name.Knockback], upgraders[BaseSkill.Name.Knockback], effectFactory);
        _skillCreaters[BaseSkill.Name.Impact] = new ImpactCreater(skillDatas[BaseSkill.Name.Impact], upgraders[BaseSkill.Name.Impact], effectFactory);

        _skillCreaters[BaseSkill.Name.SpawnBlackhole] = new SpawnBlackholeCreater(skillDatas[BaseSkill.Name.SpawnBlackhole], upgraders[BaseSkill.Name.SpawnBlackhole], weaponFactory);
        _skillCreaters[BaseSkill.Name.SpawnBlade] = new SpawnBladeCreater(skillDatas[BaseSkill.Name.SpawnBlade], upgraders[BaseSkill.Name.SpawnBlade], weaponFactory);

        _skillCreaters[BaseSkill.Name.SpawnRifleShooter] = new SpawnShooterCreater(skillDatas[BaseSkill.Name.SpawnRifleShooter], upgraders[BaseSkill.Name.SpawnRifleShooter], weaponFactory);
        _skillCreaters[BaseSkill.Name.SpawnRocketShooter] = new SpawnShooterCreater(skillDatas[BaseSkill.Name.SpawnRocketShooter], upgraders[BaseSkill.Name.SpawnRocketShooter], weaponFactory);

        _skillCreaters[BaseSkill.Name.SpawnStickyBomb] = new SpawnStickyBombCreater(skillDatas[BaseSkill.Name.SpawnStickyBomb], upgraders[BaseSkill.Name.SpawnStickyBomb], weaponFactory);

        _skillCreaters[BaseSkill.Name.ContactAttack] = new ContactAttackCreater(skillDatas[BaseSkill.Name.ContactAttack], effectFactory);

        _skillCreaters[BaseSkill.Name.UpgradeDamage] = new UpgradeDamageCreater(skillDatas[BaseSkill.Name.UpgradeDamage]);
        _skillCreaters[BaseSkill.Name.UpgradeCooltime] = new UpgradeCooltimeCreater(skillDatas[BaseSkill.Name.UpgradeCooltime]);
        _skillCreaters[BaseSkill.Name.UpgradeShooting] = new UpgradeShootingCreater(skillDatas[BaseSkill.Name.UpgradeShooting]);

        _skillCreaters[BaseSkill.Name.SpreadBullets] = new SpreadBulletsCreater(skillDatas[BaseSkill.Name.SpreadBullets], upgraders[BaseSkill.Name.SpreadBullets], weaponFactory);
        _skillCreaters[BaseSkill.Name.SpreadReflectableBullets] = new SpreadBulletsCreater(skillDatas[BaseSkill.Name.SpreadReflectableBullets], weaponFactory);

        _skillCreaters[BaseSkill.Name.Shockwave] = new ShockwaveCreater(skillDatas[BaseSkill.Name.Shockwave], upgraders[BaseSkill.Name.Shockwave], effectFactory);

        _skillCreaters[BaseSkill.Name.MagneticField] = new MagneticFieldCreater(skillDatas[BaseSkill.Name.MagneticField], upgraders[BaseSkill.Name.MagneticField]);

        _skillCreaters[BaseSkill.Name.SelfDestruction] = new SelfDestructionCreater(skillDatas[BaseSkill.Name.SelfDestruction], upgraders[BaseSkill.Name.SelfDestruction], effectFactory);

        _skillCreaters[BaseSkill.Name.MultipleShockwave] = new MultipleShockwaveCreater(skillDatas[BaseSkill.Name.MultipleShockwave], effectFactory);

        _skillCreaters[BaseSkill.Name.SpreadMultipleBullets] = new SpreadMultipleBulletsCreater(skillDatas[BaseSkill.Name.SpreadMultipleBullets], weaponFactory);

        _skillCreaters[BaseSkill.Name.ShootMultipleLaser] = new ShootMultipleLaserCreater(skillDatas[BaseSkill.Name.ShootMultipleLaser], effectFactory);
    }

    public override BaseSkill Create(BaseSkill.Name name)
    {
        return _skillCreaters[name].Create();
    }
}