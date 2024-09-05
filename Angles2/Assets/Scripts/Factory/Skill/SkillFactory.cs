using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BaseSkillData 
{
    public int _maxUpgradePoint;

    public BaseSkillData(int maxUpgradePoint)
    {
        _maxUpgradePoint = maxUpgradePoint;
    }
}

[Serializable] 
public class RandomSkillData : BaseSkillData
{
    public float _probability;

    public RandomSkillData(int maxUpgradePoint, float probability) : base(maxUpgradePoint)
    {
        _probability = probability;
    }
}

[Serializable] 
public class CooltimeSkillData : BaseSkillData
{
    public int _maxStackCount;
    public float _coolTime;

    public CooltimeSkillData(int maxUpgradePoint, float coolTime, int maxStackCount) : base(maxUpgradePoint)
    {
        _coolTime = coolTime;
        _maxStackCount = maxStackCount;
    }
}

abstract public class SkillCreater
{
    protected BaseSkillData _buffData;
    public SkillCreater(BaseSkillData data) { _buffData = data; }
    // 생성자에 이팩트 생성을 받는 이벤트를 추가해준다.

    public abstract BaseSkill Create();
}

public class SkillFactory : BaseFactory
{
    Dictionary<BaseSkill.Name, SkillCreater> _skillCreaters;

    public SkillFactory(Dictionary<BaseSkill.Name, BaseSkillData> skillDatas, BaseFactory effectFactory, BaseFactory weaponFactory, BaseFactory buffFactory)
    {
        _skillCreaters = new Dictionary<BaseSkill.Name, SkillCreater>();

        _skillCreaters[BaseSkill.Name.Statikk] = new StatikkCreater(skillDatas[BaseSkill.Name.Statikk], effectFactory);
        _skillCreaters[BaseSkill.Name.Knockback] = new KnockbackCreater(skillDatas[BaseSkill.Name.Knockback], effectFactory);
        _skillCreaters[BaseSkill.Name.Impact] = new ImpactCreater(skillDatas[BaseSkill.Name.Impact], effectFactory);

        _skillCreaters[BaseSkill.Name.SpawnBlackhole] = new SpawnBlackholeCreater(skillDatas[BaseSkill.Name.SpawnBlackhole], weaponFactory);
        _skillCreaters[BaseSkill.Name.SpawnBlade] = new SpawnBladeCreater(skillDatas[BaseSkill.Name.SpawnBlade], weaponFactory);

        _skillCreaters[BaseSkill.Name.SpawnRifleShooter] = new SpawnShooterCreater(skillDatas[BaseSkill.Name.SpawnRifleShooter], weaponFactory);
        _skillCreaters[BaseSkill.Name.SpawnRocketShooter] = new SpawnShooterCreater(skillDatas[BaseSkill.Name.SpawnRocketShooter], weaponFactory);

        _skillCreaters[BaseSkill.Name.SpawnStickyBomb]  = new SpawnStickyBombCreater(skillDatas[BaseSkill.Name.SpawnStickyBomb], weaponFactory);

        _skillCreaters[BaseSkill.Name.CreateTotalDamageBuff] = new CreateTotalDamageBuffCreater(skillDatas[BaseSkill.Name.CreateTotalDamageBuff], buffFactory);
        _skillCreaters[BaseSkill.Name.CreateTotalCooltimeBuff] = new CreateTotalCooltimeBuffCreater(skillDatas[BaseSkill.Name.CreateTotalCooltimeBuff], buffFactory);
        _skillCreaters[BaseSkill.Name.CreateDashBuff] = new CreateDashBuffCreater(skillDatas[BaseSkill.Name.CreateDashBuff], buffFactory);
        _skillCreaters[BaseSkill.Name.CreateShootingBuff] = new CreateShootingBuffCreater(skillDatas[BaseSkill.Name.CreateShootingBuff], buffFactory);


        _skillCreaters[BaseSkill.Name.SpreadBullets] = new SpreadBulletsCreater(skillDatas[BaseSkill.Name.SpreadBullets], weaponFactory);
        _skillCreaters[BaseSkill.Name.Shockwave] = new ShockwaveCreater(skillDatas[BaseSkill.Name.Shockwave], effectFactory);
        _skillCreaters[BaseSkill.Name.MagneticField] = new MagneticFieldCreater(skillDatas[BaseSkill.Name.MagneticField]);
        _skillCreaters[BaseSkill.Name.SelfDestruction] = new SelfDestructionCreater(skillDatas[BaseSkill.Name.SelfDestruction], effectFactory);

        _skillCreaters[BaseSkill.Name.ContactAttack] = new ContactAttackCreater(skillDatas[BaseSkill.Name.ContactAttack], effectFactory);
    }

    public override BaseSkill Create(BaseSkill.Name name)
    {
        return _skillCreaters[name].Create();
    }
}
