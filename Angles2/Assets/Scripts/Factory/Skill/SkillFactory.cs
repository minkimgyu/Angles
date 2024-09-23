using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
abstract public class SkillData 
{
    public int _maxUpgradePoint;

    public SkillData() { _maxUpgradePoint = 0; }
    public SkillData(int maxUpgradePoint)
    {
        _maxUpgradePoint = maxUpgradePoint;
    }

    public abstract SkillData Copy();
}

[Serializable]
abstract public class RandomSkillData : SkillData
{
    public float _probability;

    public RandomSkillData() { _maxUpgradePoint = 0; _probability = 0; }
    public RandomSkillData(int maxUpgradePoint, float probability) : base(maxUpgradePoint)
    {
        _probability = probability;
    }
}

[Serializable]
abstract public class CooltimeSkillData : SkillData
{
    public int _maxStackCount;
    public float _coolTime;

    public CooltimeSkillData() { _maxUpgradePoint = 0; _maxStackCount = 0; _coolTime = 0; }
    public CooltimeSkillData(int maxUpgradePoint, float coolTime, int maxStackCount) : base(maxUpgradePoint)
    {
        _coolTime = coolTime;
        _maxStackCount = maxStackCount;
    }
}

abstract public class SkillCreater
{
    protected SkillData _skillData;
    public SkillCreater(SkillData data) { _skillData = data; }
    // �����ڿ� ����Ʈ ������ �޴� �̺�Ʈ�� �߰����ش�.

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

        _skillCreaters[BaseSkill.Name.SpawnStickyBomb]  = new SpawnStickyBombCreater(skillDatas[BaseSkill.Name.SpawnStickyBomb], upgraders[BaseSkill.Name.SpawnStickyBomb], weaponFactory);

        _skillCreaters[BaseSkill.Name.ContactAttack] = new ContactAttackCreater(skillDatas[BaseSkill.Name.ContactAttack], effectFactory);

        _skillCreaters[BaseSkill.Name.UpgradeDamage] = new UpgradeDamageCreater(skillDatas[BaseSkill.Name.UpgradeDamage]);
        _skillCreaters[BaseSkill.Name.UpgradeCooltime] = new UpgradeCooltimeCreater(skillDatas[BaseSkill.Name.UpgradeCooltime]);
        _skillCreaters[BaseSkill.Name.UpgradeDash] = new UpgradeDashCreater(skillDatas[BaseSkill.Name.UpgradeDash]);
        _skillCreaters[BaseSkill.Name.UpgradeShooting] = new UpgradeShootingCreater(skillDatas[BaseSkill.Name.UpgradeShooting]);


        _skillCreaters[BaseSkill.Name.SpreadBullets] = new SpreadBulletsCreater(skillDatas[BaseSkill.Name.SpreadBullets], upgraders[BaseSkill.Name.SpreadBullets], weaponFactory);

        _skillCreaters[BaseSkill.Name.Shockwave] = new ShockwaveCreater(skillDatas[BaseSkill.Name.Shockwave], upgraders[BaseSkill.Name.Shockwave], effectFactory);

        _skillCreaters[BaseSkill.Name.MagneticField] = new MagneticFieldCreater(skillDatas[BaseSkill.Name.MagneticField], upgraders[BaseSkill.Name.MagneticField]);

        _skillCreaters[BaseSkill.Name.SelfDestruction] = new SelfDestructionCreater(skillDatas[BaseSkill.Name.SelfDestruction], upgraders[BaseSkill.Name.SelfDestruction], effectFactory);

        _skillCreaters[BaseSkill.Name.MultipleShockwave] = new MultipleShockwaveCreater(skillDatas[BaseSkill.Name.MultipleShockwave], effectFactory);
    }

    public override BaseSkill Create(BaseSkill.Name name)
    {
        return _skillCreaters[name].Create();
    }
}
